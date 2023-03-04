namespace CompetitiveTennis.Tournaments.Mapping;

using System.Text.Json;
using Data.Models;
using Mapster;
using Models;
using Models.Avenue;

public class AvenueMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Avenue, AvenueOutputModel>()
            .Map(dest => dest.Courts,
                src => JsonSerializer.Deserialize<List<CourtsInfo>>(src.Courts, SerializerOptions.StringEnumOptions()));
    }
}