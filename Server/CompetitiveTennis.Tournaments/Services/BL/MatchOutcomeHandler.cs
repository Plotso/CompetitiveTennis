namespace CompetitiveTennis.Tournaments.Services.BL;

using CompetitiveTennis.Data.Models.Enums;
using Contracts.Match;
using Extensions;
using Interfaces.BL;
using Interfaces.Data;
using Models;
using Models.MatchOutcomeHandler;
using Models.MatchOutcomeHandler.RatingCalculations;
using Tournaments.Data;

public class MatchOutcomeHandler : IMatchOutcomeHandler
{
    private readonly IMatchesService _matchesService;
    private readonly IRatingCalculator _ratingCalculator;
    private readonly IAccountsService _accountsService;
    private readonly IParticipantsService _participantsService;
    private readonly ILogger<MatchOutcomeHandler> _logger;

    public MatchOutcomeHandler(IMatchesService matchesService, IRatingCalculator ratingCalculator, IAccountsService accountsService, IParticipantsService participantsService, ILogger<MatchOutcomeHandler> logger)
    {
        _matchesService = matchesService;
        _ratingCalculator = ratingCalculator;
        _accountsService = accountsService;
        _participantsService = participantsService;
        _logger = logger;
    }
    /// <summary>
    /// Update match outcome, recalculate player ratings & update participants for successor match
    /// </summary>
    public async Task HandleMatchOutcome(int matchId, MatchResultsInputModel matchResultsInputModel)
    {
        if (matchResultsInputModel == null)
            return;
        var previousMatchOutcome = await _matchesService.GetMatchOutcome(matchId);
        var matchOutcome = GetMatchOutcome(matchResultsInputModel);
        await _matchesService.UpdateOutcomeAndStatus(matchId, matchOutcome, status: matchResultsInputModel.IsEnded ? EventStatus.Ended : EventStatus.InProgress);
        var hasSuccessorMatchEnded = await HasSuccessorMatchEnded(matchId); //If successor match has ended, player ratings should be adjusted since this will mess up everything afterwards
        if (matchResultsInputModel.IsEnded && !matchResultsInputModel.MatchPeriods.IsNullOrEmpty() && !hasSuccessorMatchEnded)
            await UpdateRatingsAndSuccessorMatchParticipants(matchId, previousMatchOutcome);
        if (!matchResultsInputModel.IsEnded && previousMatchOutcome != MatchOutcome.NoOutcome)
            await RollbackPlayerRatings(matchId);
    }

    public async Task HandleMatchOutcome(int matchId, MatchCustomConditionResultInputModel matchCustomConditionResultInputModel)
    {
        if (matchCustomConditionResultInputModel == null)
            return;
        var previousMatchOutcome = await _matchesService.GetMatchOutcome(matchId);
        await _matchesService.UpdateOutcomeWithCondition(matchId, matchCustomConditionResultInputModel.MatchOutcome, matchCustomConditionResultInputModel.OutcomeCondition, status: EventStatus.Ended);    
        //ToDo: InDepth revise the code below
        var hasSuccessorMatchEnded = await HasSuccessorMatchEnded(matchId); //If successor match has ended, player ratings should be adjusted since this will mess up everything afterwards
        if (hasSuccessorMatchEnded)
            await UpdateRatingsAndSuccessorMatchParticipants(matchId, previousMatchOutcome);
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
        return await HasSuccessorMatchStarted(matchId);
    }

    public async Task<bool> HasSuccessorMatchStarted(int matchId)
    {
        var matchFlow = await _matchesService.GetMatchFlow(matchId);
        if (matchFlow is null) 
            return false;
        var hasSuccessorMatchStarted = await _matchesService.HasMatchStarted(matchFlow.SuccessorMatchId);
        return hasSuccessorMatchStarted.HasValue && hasSuccessorMatchStarted.Value;
    }

    public async Task<bool> HasSuccessorMatchEnded(int matchId)
    {
        var matchFlow = await _matchesService.GetMatchFlow(matchId);
        if (matchFlow is null) 
            return false;
        var hasSuccessorMatchStarted = await _matchesService.HasMatchEnded(matchFlow.SuccessorMatchId);
        return hasSuccessorMatchStarted.HasValue && hasSuccessorMatchStarted.Value;
    }

    private async Task UpdateRatingsAndSuccessorMatchParticipants(int matchId, MatchOutcome? previousOutcomeOfTheMatch)
    {
        var slimMatchOutputModel = await _matchesService.GetForRatingCalculations(matchId);
        var matchResultSummaryWithRatings = GetMatchResultSummaryWithRatings(slimMatchOutputModel);
        var isDoubles = await _matchesService.IsDoubles(matchId);
        var hasOutcomeUpdate = !previousOutcomeOfTheMatch.HasValue || previousOutcomeOfTheMatch != matchResultSummaryWithRatings.Outcome;
        var hasPreviousWinner = previousOutcomeOfTheMatch == MatchOutcome.ParticipantOne || previousOutcomeOfTheMatch == MatchOutcome.ParticipantTwo;
        var ratingsRollback = false;
        if(hasOutcomeUpdate && hasPreviousWinner && (isDoubles == null || !isDoubles.Value)) //sadly, for doubles matches new mechanism for rollbacks should be implemented
        {
            //ToDo: Rollback player ratings
            var matchParticipantsInfo = await _matchesService.GetMatchParticipantsInfo(matchId);
            if (!matchParticipantsInfo.IsNullOrEmpty())
            {
                matchResultSummaryWithRatings = matchResultSummaryWithRatings with
                {
                    Participants = RollbackMatchParticipantRatingInOutputModel(matchResultSummaryWithRatings.Participants, matchParticipantsInfo, isDoubles: isDoubles ?? false)
                };

                ratingsRollback = true;
            }
        }
        var updatedRating = _ratingCalculator.CalculateRatings(matchResultSummaryWithRatings, isDoubles: isDoubles ?? false);
        if (hasOutcomeUpdate || ratingsRollback) // prevent rating updates when same score is resubmitted multiple times
            foreach(var accountRating in updatedRating)
                await _accountsService.UpdatePlayerRating(accountRating.AccountId, accountRating.NewRating, isDoubles: isDoubles ?? false);
        await UpdateSuccessorMatchParticipant(matchId, slimMatchOutputModel, matchResultSummaryWithRatings);
    }

    private async Task RollbackPlayerRatings(int matchId)
    {
        var isDoubles = await _matchesService.IsDoubles(matchId);
        if (isDoubles ?? false) //sadly, for doubles matches new mechanism for rollbacks should be implemented
            return;
        var matchParticipantsInfo = await _matchesService.GetMatchParticipantsInfo(matchId);
        foreach (var participantRatingInfo in matchParticipantsInfo)
        {
            var participant = await _participantsService.GetInternal(participantRatingInfo.ParticipantId);
            if (participant is not null)
            {
                if (participant.Players.Count() > 1) //abort since only singles should be updated
                {
                    _logger.LogError($"Aborting {nameof(RollbackPlayerRatings)} due to participant with more than 1 account linked to it. MatchId: {matchId}.");
                    return;
                }

                var accountParticipant = participant.Players.FirstOrDefault();
                if (accountParticipant is not null && participantRatingInfo.PrematchRating.HasValue)
                {
                    await _accountsService.UpdatePlayerRating(accountParticipant.Account.Id, participantRatingInfo.PrematchRating.Value, isDoubles: isDoubles ?? false);
                }
            }
        }
    }

    /// <summary>
    /// Doubles are not supported for rating rollbacks at the moment due to approach limitations
    /// </summary>
    private IEnumerable<ParticipantRatingOutputModel> RollbackMatchParticipantRatingInOutputModel(IEnumerable<ParticipantRatingOutputModel> participants, IEnumerable<MatchParticipantRatingInfo> matchParticipantsInfo, bool isDoubles)
    {
        if (isDoubles || matchParticipantsInfo.IsNullOrEmpty() || participants.IsNullOrEmpty() || matchParticipantsInfo.Count() != participants.Count())
            return participants;
        
        var result = new List<ParticipantRatingOutputModel>(participants.Count());
        foreach (var participant in participants)
        {
            var matchParticipantInfo = matchParticipantsInfo.FirstOrDefault(mp => mp.ParticipantId == participant.Id);
            if (matchParticipantInfo is null)
                return participants;
            var accounts = participant.Players;
            if (!isDoubles && accounts.Count() > 1)
            {
                _logger.LogError($"Aborting {nameof(RollbackMatchParticipantRatingInOutputModel)} due to participant with more than 1 account linked to it. ParticipantId: {participant.Id}.");
                return participants;
            }
            if (accounts.Count() == 1)
            {
                var accountInfo = accounts.Single();
                var playersRating = isDoubles ? accountInfo.PlayerRating : matchParticipantInfo.PrematchRating;
                var doublesRating = accountInfo.DoublesPlayerRating; //ToDo: This needs to be updated once rollback of doubles rating is supported
                result.Add(participant with { Players = new[] { new AccountRatingOutputModel(accountInfo.Id, playersRating ?? accountInfo.PlayerRating, doublesRating ) } });
            }
            else
                result.Add(participant);
        }

        return result;
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
        if (!matchResultsInputModel.IsEnded || EnumerableExtensions.IsNullOrEmpty(matchResultsInputModel.MatchPeriods))
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