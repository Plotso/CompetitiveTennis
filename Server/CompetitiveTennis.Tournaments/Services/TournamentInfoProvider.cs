namespace CompetitiveTennis.Tournaments.Services;

using Contracts.Account;
using Contracts.Match;
using Contracts.Participant;
using Contracts.Tournament;
using Data;
using Models;

public static class TournamentInfoProvider
{
    public static SlimTournamentOutputModel GetTournamentInfo(FullTournamentOutputModel outputModel)
    {
        var matches = new List<MatchShortOutputModel>();
        foreach (var matchOuput in outputModel.Matches)
        {
            var predecesorMatchFlows = outputModel.MatchFlows.Where(mf => mf.SuccessorMatchId == matchOuput.Id);
            var homePredecesorMatch = predecesorMatchFlows.FirstOrDefault(mf => mf.IsHome);
            var awayPredecesorMatch = predecesorMatchFlows.FirstOrDefault(mf => !mf.IsHome);

            var homeParticipant =
                matchOuput.Participants.FirstOrDefault(p => p.Specifier == DataConstants.ParticipantSpecifiers.Home);
            var awayParticipant =
                matchOuput.Participants.FirstOrDefault(p => p.Specifier == DataConstants.ParticipantSpecifiers.Away);

            var homeParticipantInfo = homeParticipant != null
                ? new ParticipantInfo(homeParticipant.Id, homeParticipant.IsGuest, GetName(homeParticipant),
                    homeParticipant.Players, homeParticipant.Specifier, homeParticipant.Team)
                : new ParticipantInfo(-1, true, $"Winner of match {homePredecesorMatch?.MatchId}", new AccountShortOutputModel[0], DataConstants.ParticipantSpecifiers.Home, null);
            var awayParticipantInfo = awayParticipant != null
                ? new ParticipantInfo(awayParticipant.Id, awayParticipant.IsGuest, GetName(awayParticipant),
                    awayParticipant.Players, awayParticipant.Specifier, awayParticipant.Team)
                : new ParticipantInfo(-2, true, $"Winner of match {awayPredecesorMatch?.MatchId}", new AccountShortOutputModel[0], DataConstants.ParticipantSpecifiers.Away, null);

            matches.Add(new MatchShortOutputModel(matchOuput.Id, matchOuput.StartDate, matchOuput.MatchWonPoints,
                matchOuput.SetWonPoints, matchOuput.GameWonPoints, matchOuput.Stage,
                matchOuput.Details, matchOuput.Status, matchOuput.Outcome, OutcomeCondition: null, homePredecesorMatch?.MatchId,
                awayPredecesorMatch?.MatchId,
                homeParticipantInfo, awayParticipantInfo, matchOuput.Scores));
        }

        return new SlimTournamentOutputModel(outputModel.Id, outputModel.Title, outputModel.Rules,
            outputModel.Description, outputModel.Type, outputModel.Surface,
            outputModel.EntryFee, outputModel.Prize, outputModel.CourtsAvailable, outputModel.MinParticipants,
            outputModel.MaxParticipants,
            outputModel.MatchWonPoints, outputModel.SetWonPoints, outputModel.GameWonPoints, outputModel.StartDate,
            outputModel.EndDate,
            outputModel.CreatedOn, outputModel.ModifiedOn, outputModel.Avenue, outputModel.Organiser,
            outputModel.Participants, matches);
    }

    private static string GetName(ParticipantShortOutputModel participantShortOutputModel)
    {
        if (participantShortOutputModel?.Players == null || !participantShortOutputModel.Players.Any())
            return participantShortOutputModel.Name;

        var playerNames = string.Join(" ,",
            participantShortOutputModel.Players.Select(p =>
                $"{p.FirstName} {p.LastName} ({p.Username} | {p.PlayerRating})"));
        return participantShortOutputModel.IsGuest ? $"{participantShortOutputModel.Name}, {playerNames}" : playerNames;
    }
}