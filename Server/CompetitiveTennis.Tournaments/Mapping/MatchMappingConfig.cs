namespace CompetitiveTennis.Tournaments.Mapping;

using Contracts.Account;
using Contracts.Match;
using Contracts.Participant;
using Contracts.Team;
using Data.Models;
using Mapster;

public class MatchMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Match, MatchOutputModel>()
            .Map(dest => dest.Participants,
                src => MapParticipants(src));
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
}