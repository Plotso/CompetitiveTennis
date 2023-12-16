namespace CompetitiveTennis.Tournaments.Mapping;

using Contracts.Match;
using Contracts.Participant;
using Data.Models;
using Mapster;

public class MatchMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Match, MatchOutputModel>()
            .Map(dest => dest.Participants,
                src => src.Participants.Select(p => p.Participant.Adapt<ParticipantShortOutputModel>()));
    }
}