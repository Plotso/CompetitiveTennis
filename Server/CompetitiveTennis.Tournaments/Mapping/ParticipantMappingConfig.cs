namespace CompetitiveTennis.Tournaments.Mapping;

using Contracts.Account;
using Contracts.Match;
using Contracts.Participant;
using Data.Models;
using Mapster;

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
                src => src.Players.Select(p => p.Account.Adapt<AccountShortOutputModel>()));
    }
}