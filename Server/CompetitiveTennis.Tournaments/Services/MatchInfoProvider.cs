namespace CompetitiveTennis.Tournaments.Services;

using CompetitiveTennis.Data.Models.Enums;
using Contracts.Account;
using Contracts.Match;
using Contracts.MatchPeriod;
using Contracts.Participant;
using Extensions;
using Models;
using Models.MatchOutcomeHandler;
using Tournaments.Data;

public static class MatchInfoProvider
{
    public static MatchShortOutputModel? GetMatchInfoFromTournament(MatchOutputModel matchOutput, TournamentMatchFlowInfo tournamentMatchFlowInfo)
    {
        if (matchOutput == null)
            return null;
        
        var predecesorMatchFlows = tournamentMatchFlowInfo.MatchFlows.Where(mf => mf.SuccessorMatchId == matchOutput.Id);
        var homePredecesorMatch = predecesorMatchFlows.FirstOrDefault(mf => mf.IsHome);
        var awayPredecesorMatch = predecesorMatchFlows.FirstOrDefault(mf => !mf.IsHome);

        var homeParticipant =
            matchOutput.Participants.FirstOrDefault(p => p.Specifier == DataConstants.ParticipantSpecifiers.Home);
        var awayParticipant =
            matchOutput.Participants.FirstOrDefault(p => p.Specifier == DataConstants.ParticipantSpecifiers.Away);

        var homeParticipantInfo = homeParticipant != null
            ? new ParticipantInfo(homeParticipant.Id, homeParticipant.IsGuest, GetName(homeParticipant),
                homeParticipant.Players, homeParticipant.Specifier, homeParticipant.Team)
            : new ParticipantInfo(-1, true, $"Winner of match {homePredecesorMatch?.MatchId}", new AccountShortOutputModel[0], DataConstants.ParticipantSpecifiers.Home, null);
        var awayParticipantInfo = awayParticipant != null
            ? new ParticipantInfo(awayParticipant.Id, awayParticipant.IsGuest, GetName(awayParticipant),
                awayParticipant.Players, awayParticipant.Specifier, awayParticipant.Team)
            : new ParticipantInfo(-2, true, $"Winner of match {awayPredecesorMatch?.MatchId}", new AccountShortOutputModel[0], DataConstants.ParticipantSpecifiers.Away, null);

        return new MatchShortOutputModel(matchOutput.Id, matchOutput.StartDate, matchOutput.MatchWonPoints,
            matchOutput.SetWonPoints, matchOutput.GameWonPoints, matchOutput.Stage,
            matchOutput.Details, matchOutput.Status, matchOutput.Outcome, OutcomeCondition: null,
            homePredecesorMatch?.MatchId,
            awayPredecesorMatch?.MatchId,
            homeParticipantInfo, awayParticipantInfo, matchOutput.MatchPeriods, tournamentMatchFlowInfo.Id, GetMatchResults(matchOutput.MatchPeriods));
    }

    private static string GetName(ParticipantShortOutputModel participantShortOutputModel)
    {
        if (participantShortOutputModel?.Players == null || !participantShortOutputModel.Players.Any())
            return participantShortOutputModel.Name;

        var playerNames = string.Join(" ,",
            participantShortOutputModel.Players.Select(p =>
                $"{p.FirstName} {p.LastName} ({p.Username} | {p.PlayerRating})"));  //ToDo: Revise those brackets styling since this results in Odd results on FE
        return participantShortOutputModel.IsGuest ? $"{participantShortOutputModel.Name}, {playerNames}" : playerNames;
    }

    public static MatchResultsOutputModel? GetMatchResults(IEnumerable<MatchPeriodOutputModel> matchPeriods)
    {
        var periodsInfo = matchPeriods.Select(MatchPeriodShortInfo.FromMatchPeriodInput).ToArray();
        if (periodsInfo.IsNullOrEmpty())
            return null;
        var periodsBySet = periodsInfo.GroupBy(mp => mp.Set)
            .ToDictionary(grp => grp.Key, grp => grp.ToList());
        
        var sets = new List<SetResultOutput>(periodsBySet.Keys.Count);
        sbyte setCounter = 1;
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
            sets.Add(new SetResultOutput(setCounter++, GetSetWinner(homeSideGames, awaySideGames), homeSideGames, awaySideGames));
        }
        return new MatchResultsOutputModel(sets);
    }
    
    private static MatchOutcome GetSetWinner(ushort homeSideGames, ushort awaySideGames) 
        => homeSideGames > awaySideGames ? MatchOutcome.ParticipantOne :
            awaySideGames > homeSideGames ? MatchOutcome.ParticipantTwo : MatchOutcome.NoOutcome;
}