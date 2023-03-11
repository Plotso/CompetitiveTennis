namespace CompetitiveTennis.Tournaments.Mapping;

using Data.Models;
using Mapster;
using Models.Account;
using Models.Match;
using Models.Participant;

public class ParticipantMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Participant, ParticipantOutputModel>()
            .Map(dest => dest.Matches,
                src => src.Matches.Select(m => m.Match.Adapt<MatchOutputModel>()))
            .Map(dest => dest.Players, 
                src => src.Players.Select(p => p.Account.Adapt<AccountOutputModel>()));
        config.NewConfig<Participant, ParticipantShortOutputModel>()
            .Map(dest => dest.Players, 
                src => src.Players.Select(p => p.Account.Adapt<AccountOutputModel>()));
    }
}