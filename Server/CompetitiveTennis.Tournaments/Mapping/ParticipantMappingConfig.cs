namespace CompetitiveTennis.Tournaments.Mapping;

using Data.Models;
using Mapster;
using Models.Match;
using Models.Participant;

public class ParticipantMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Participant, ParticipantOutputModel>()
            .Map(dest => dest.Matches,
                src => src.Matches.Select(m => m.Match.Adapt<MatchOutputModel>()));
    }
}