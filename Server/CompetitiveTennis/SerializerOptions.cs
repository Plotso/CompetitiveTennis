namespace CompetitiveTennis;

using System.Text.Json;
using System.Text.Json.Serialization;

public static class SerializerOptions
{
    public static JsonSerializerOptions StringEnumOptions()
    {
        var options = JsonSerializerOptions.Default;
        if (!options.Converters.Any(c => c.GetType() == typeof(JsonStringEnumConverter)))
            options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}