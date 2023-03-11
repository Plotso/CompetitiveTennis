namespace CompetitiveTennis.Tournaments.SerializerOptions;

using System.Text.Json;
using System.Text.Json.Serialization;

internal class SerializerOptions : ISerializerOptions
{
    private readonly JsonSerializerOptions _options;

    public SerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        options.Converters.Add(new JsonStringEnumConverter());
        _options = options;
    }

    public JsonSerializerOptions GetSerializerOptions() => _options;
}