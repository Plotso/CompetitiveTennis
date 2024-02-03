namespace CompetitiveTennis.Tournaments.Services;

using Contracts.Account;
using Contracts.Match;
using Contracts.Participant;
using Contracts.Tournament;
using Data;
using Models;

public static class MatchInfoProvider
{
    public static MatchShortOutputModel? GetMatchInfoFromTournament(FullTournamentOutputModel outputModel, int matchId)
    {
        var matchOutput = outputModel.Matches.FirstOrDefault(m => m.Id == matchId);
        if (matchOutput == null)
            return null;
        
        var predecesorMatchFlows = outputModel.MatchFlows.Where(mf => mf.SuccessorMatchId == matchOutput.Id);
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
            homeParticipantInfo, awayParticipantInfo, matchOutput.Scores, outputModel.Id);
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
}