namespace CompetitiveTennis.Tournaments.Mapping;

using System.Text.Json;
using System.Text.Json.Serialization;
using Data.Models;
using Mapster;
using Models;
using Models.Avenue;

public class AvenueMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter());
        
        config.NewConfig<Avenue, AvenueOutputModel>()
            .Map(dest => dest.Courts,
                src => JsonSerializer.Deserialize<List<CourtsInfo>>(src.Courts, options));

        config.NewConfig<AvenueInputModel, Avenue>()
            .Map(dest => dest.Courts,
                src => JsonSerializer.Serialize(src.Courts, options));
    }
}