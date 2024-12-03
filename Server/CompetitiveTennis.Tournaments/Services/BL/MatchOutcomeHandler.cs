namespace CompetitiveTennis.Tournaments.Services.BL;

using CompetitiveTennis.Data.Models.Enums;
using Contracts.Match;
using Extensions;
using Interfaces.BL;
using Interfaces.Data;
using Models.MatchOutcomeHandler;
using Models.MatchOutcomeHandler.RatingCalculations;
using Tournaments.Data;

public class MatchOutcomeHandler : IMatchOutcomeHandler
{
    private readonly IMatchesService _matchesService;
    private readonly IRatingCalculator _ratingCalculator;
    private readonly IAccountsService _accountsService;
    private readonly IParticipantsService _participantsService;

    public MatchOutcomeHandler(IMatchesService matchesService, IRatingCalculator ratingCalculator, IAccountsService accountsService, IParticipantsService participantsService)
    {
        _matchesService = matchesService;
        _ratingCalculator = ratingCalculator;
        _accountsService = accountsService;
        _participantsService = participantsService;
    }
    /// <summary>
    /// Update match outcome, recalculate player ratings & update participants for successor match
    /// </summary>
    public async Task HandleMatchOutcome(int matchId, MatchResultsInputModel matchResultsInputModel)
    {
        if (matchResultsInputModel == null)
            return;
        var matchOutcome = GetMatchOutcome(matchResultsInputModel);
        await _matchesService.UpdateOutcome(matchId, matchOutcome);
        if (matchResultsInputModel.IsEnded && !matchResultsInputModel.MatchPeriods.IsNullOrEmpty())
            await UpdateRatingsAndSuccessorMatchParticipants(matchId);
        
    }

    public async Task HandleMatchOutcome(int matchId, MatchCustomConditionResultInputModel matchCustomConditionResultInputModel)
    {
        if (matchCustomConditionResultInputModel == null)
            return;
        await _matchesService.UpdateOutcomeWithCondition(matchId, matchCustomConditionResultInputModel.MatchOutcome, matchCustomConditionResultInputModel.OutcomeCondition);    
        //ToDo: InDepth revise the code below
        await UpdateRatingsAndSuccessorMatchParticipants(matchId);
    }

    /// <summary>
    /// User should be able to update scores of old matches as long as the winner is not changed in cases when the winner has already started the successor match
    /// </summary>
    public async Task<bool> IsChangeOfWinnerForMatchWithAlreadyStartedSuccessorMatch(int matchId, MatchResultsInputModel matchResultsInputModel)
    {
        if (matchResultsInputModel == null)
            return false;
        
        var matchOutcome = GetMatchOutcome(matchResultsInputModel);
        return await IsChangeOfWinnerForMatchWithAlreadyStartedSuccessorMatch(matchId, matchOutcome);
    }

    public async Task<bool> IsChangeOfWinnerForMatchWithAlreadyStartedSuccessorMatch(int matchId, MatchOutcome updatedMatchOutcome)
    {
        var oldOutcome = await _matchesService.GetMatchOutcome(matchId);
        if (oldOutcome == null || oldOutcome == MatchOutcome.NoOutcome)
            return false;
        var hasOutcomeChange = updatedMatchOutcome != oldOutcome;
        if (!hasOutcomeChange)
            return false;
        var matchFlow = await _matchesService.GetMatchFlow(matchId);
        if (matchFlow is null) 
            return false;
        var hasSuccessorMatchStarted = await _matchesService.HasMatchStarted(matchFlow.SuccessorMatchId);
        return hasOutcomeChange && hasSuccessorMatchStarted.HasValue && hasSuccessorMatchStarted.Value;
    }

    private async Task UpdateRatingsAndSuccessorMatchParticipants(int matchId)
    {
        var slimMatchOutputModel = await _matchesService.GetForRatingCalculations(matchId);
        var matchResultSummaryWithRatings = GetMatchResultSummaryWithRatings(slimMatchOutputModel);
        var isDoubles = await _matchesService.IsDoubles(matchId);
        var updatedRating = _ratingCalculator.CalculateRatings(matchResultSummaryWithRatings, isDoubles: isDoubles ?? false);
        foreach(var accountRating in updatedRating)
        {
            await _accountsService.UpdatePlayerRating(accountRating.AccountId, accountRating.NewRating);
            await UpdateSuccessorMatchParticipant(matchId, slimMatchOutputModel, matchResultSummaryWithRatings);
        }
    }

    private async Task UpdateSuccessorMatchParticipant(int matchId, SlimMatchOutputModel slimMatchOutputModel,
        MatchResultSummaryWithRatings matchResultSummaryWithRatings)
    {
        if (slimMatchOutputModel.Stage == TournamentStage.Final || matchResultSummaryWithRatings.Outcome == MatchOutcome.NoOutcome)
            return;
        
        var matchFlow = await _matchesService.GetMatchFlow(matchId);
        if (matchFlow is null) 
            return;
        var participantSpecifier = matchResultSummaryWithRatings.Outcome == MatchOutcome.ParticipantOne
            ? DataConstants.ParticipantSpecifiers.Home
            : DataConstants.ParticipantSpecifiers.Away;
        var winningParticipant =
            matchResultSummaryWithRatings.Participants.First(p => p.Specifier == participantSpecifier);
        var participantDbModel = await _participantsService.GetInternal(winningParticipant.Id);
        if (participantDbModel is null) 
            return;
        await _matchesService.UpdateParticipant(matchFlow.SuccessorMatchId, participantDbModel, matchFlow.IsHome);
    }

    private MatchOutcome GetMatchOutcome(MatchResultsInputModel matchResultsInputModel)
    {
        if (!matchResultsInputModel.IsEnded || matchResultsInputModel.MatchPeriods.IsNullOrEmpty())
            return MatchOutcome.NoOutcome;

        var periodsInfo = matchResultsInputModel.MatchPeriods.Select(MatchPeriodShortInfo.FromMatchPeriodInput).ToArray();
        var periodsBySet = periodsInfo.GroupBy(mp => mp.Set)
            .ToDictionary(grp => grp.Key, grp => grp.ToList());

        byte homeSideSets = 0;
        byte awaySideSets = 0;

        foreach (var setPeriods in periodsBySet)
        {
            var periodsPerGameInSet = setPeriods.Value.GroupBy(mp => mp.Game).ToDictionary(mpg => mpg.Key, mpg => mpg.ToList());
            ushort homeSideGames = 0;
            ushort awaySideGames = 0;
            foreach (var gamePeriods in periodsPerGameInSet)
            {
                var homeSideWinCount = gamePeriods.Value.Count(mp => mp.Winner == MatchOutcome.ParticipantOne);
                var awaySideWinCount = gamePeriods.Value.Count(mp => mp.Winner == MatchOutcome.ParticipantTwo);
                if (homeSideWinCount > awaySideWinCount)
                    homeSideGames++;
                if(awaySideWinCount > homeSideWinCount)
                    awaySideGames++;
            }

            if (homeSideGames > awaySideGames)
                homeSideSets++;
            if (awaySideGames > homeSideGames)
                awaySideSets++;
        }

        if (homeSideSets == awaySideSets)
            return MatchOutcome.NoOutcome;

        return homeSideSets > awaySideSets ? MatchOutcome.ParticipantOne : MatchOutcome.ParticipantTwo;
    }

    private MatchResultSummaryWithRatings GetMatchResultSummaryWithRatings(SlimMatchOutputModel match)
    {
        byte homeSideSets = 0;
        byte awaySideSets = 0;
        ushort homeSideTotalGames = 0;
        ushort awaySideTotalGames = 0;
        int homeSideTotalPoints = 0;
        int awaySideTotalPoints = 0;
        
        var periodsBySet = match.MatchPeriods.GroupBy(mp => mp.Set)
            .ToDictionary(grp => grp.Key, grp => grp.ToList());

        foreach (var setPeriods in periodsBySet)
        {
            var periodsPerGameInSet = setPeriods.Value.GroupBy(mp => mp.Game).ToDictionary(mpg => mpg.Key, mpg => mpg.ToList());
            ushort homeSideGames = 0;
            ushort awaySideGames = 0;
            foreach (var gamePeriods in periodsPerGameInSet)
            {
                var homeSideWinCount = gamePeriods.Value.Count(mp => mp.Winner == MatchOutcome.ParticipantOne);
                var awaySideWinCount = gamePeriods.Value.Count(mp => mp.Winner == MatchOutcome.ParticipantTwo);
                if (homeSideWinCount > awaySideWinCount)
                {
                    homeSideGames++;
                    homeSideTotalGames++;
                }

                if(awaySideWinCount > homeSideWinCount)
                {
                    awaySideGames++;
                    awaySideTotalGames++;
                }
                
                homeSideTotalPoints += gamePeriods.Value.SelectMany(gp => gp.Scores).Count(s => s is {PointWinner: MatchOutcome.ParticipantOne, IsWinningPoint: false});
                awaySideTotalPoints += gamePeriods.Value.SelectMany(gp => gp.Scores).Count(s => s is {PointWinner: MatchOutcome.ParticipantTwo, IsWinningPoint: false});
                
            }

            if (homeSideGames > awaySideGames)
                homeSideSets++;
            if (awaySideGames > homeSideGames)
                awaySideSets++;
        }

        return new MatchResultSummaryWithRatings(
            match.Id, match.Outcome, new TennisResultInfo(homeSideSets, homeSideTotalGames, (ushort)homeSideTotalPoints), new TennisResultInfo(awaySideSets, awaySideTotalGames, (ushort)awaySideTotalPoints), match.Participants);
    }
}