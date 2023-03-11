namespace CompetitiveTennis.Tournaments.Mapping;

using Data.Models;
using Mapster;
using Models.Match;
using Models.Participant;

public class MatchMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Match, MatchOutputModel>()
            .Map(dest => dest.Participants,
                src => src.Participants.Select(p => p.Participant.Adapt<ParticipantShortOutputModel>()));
    }
}