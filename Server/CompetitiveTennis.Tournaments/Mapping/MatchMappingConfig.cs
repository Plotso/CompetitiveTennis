namespace CompetitiveTennis.Tournaments.Mapping;

using Contracts.Match;
using Contracts.Participant;
using Data.Models;
using Extensions;
using Mapster;
using Models.MatchOutcomeHandler.RatingCalculations;

public class MatchMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Match, MatchOutputModel>()
            .Map(dest => dest.Participants,
                src => MapParticipants(src))
            .Map(dest => dest.MatchPeriods, src => src.Periods);
        config.NewConfig<Match, SlimMatchOutputModel>()
            .Map(dest => dest.Participants,
                src => MapParticipantsToParticipantRatingOutputModels(src))
            .Map(dest => dest.MatchPeriods, src => src.Periods);
    }

    private List<ParticipantShortOutputModel> MapParticipants(Match source)
    {
        if (source.Participants == null)
            return null;
        var participantModels = new List<ParticipantShortOutputModel>();

        foreach (var participantMatch in source.Participants)
        {
            /*
            var participantModel = new ParticipantShortOutputModel
            (
                Id : participantMatch.Participant.Id,
                Name : participantMatch.Participant.Name,
                Points : participantMatch.Participant.Points,
                IsGuest : participantMatch.Participant.IsGuest,
                Players : participantMatch.Participant.Players.Adapt<IEnumerable<AccountShortOutputModel>>(),
                Specifier : participantMatch.Specifier,
                Team : participantMatch.Participant.Team?.Adapt<TeamShortOutputModel>()
            );
            */
            
            var participantModel = participantMatch.Participant.Adapt<ParticipantShortOutputModel>();
            participantModel = participantModel with {Specifier = participantMatch.Specifier};

            participantModels.Add(participantModel);
        }

        return participantModels;
    }

    private List<ParticipantRatingOutputModel> MapParticipantsToParticipantRatingOutputModels(Match source) 
        => source.Participants == null ?
            null :
            source.Participants.Select(participantMatch => new ParticipantRatingOutputModel(participantMatch.Participant.Id, participantMatch.Participant.IsGuest, participantMatch.Specifier, ConvertAccounts(participantMatch.Participant.Players))).ToList();

    private static AccountRatingOutputModel[] ConvertAccounts(ICollection<AccountParticipant> participantAccounts) 
        => participantAccounts.IsNullOrEmpty() ?
            Array.Empty<AccountRatingOutputModel>() :
            participantAccounts.Select(pa => new AccountRatingOutputModel(pa.Account.Id, pa.Account.PlayerRating, pa.Account.DoublesRating)).ToArray();
}