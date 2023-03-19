namespace CompetitiveTennis.Tournaments.Mapping;

using Data.Models;
using Mapster;
using Models.Account;
using Models.Participant;

public class AccountMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Account, AccountOutputModel>()
            .Map(dest => dest.Participations,
                src => src.Participations.Select(p => p.Participant.Adapt<ParticipantShortOutputModel>()));
    }
}