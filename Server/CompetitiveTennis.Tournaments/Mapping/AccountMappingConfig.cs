namespace CompetitiveTennis.Tournaments.Mapping;

using Contracts.Account;
using Contracts.Participant;
using Data.Models;
using Mapster;
using Models;

public class AccountMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Account, AccountOutputModel>()
            .Map(dest => dest.Participations,
                src => src.Participations.Select(p => p.Participant.Adapt<ParticipantShortOutputModel>()));
        config.NewConfig<Account, AccountRatingInfo>()
            .Map(dest => dest.SinglesRating, src => src.PlayerRating);
    }
}