namespace CompetitiveTennis.Tournaments.SerializerOptions;

using System.Text.Json;

public interface ISerializerOptions
{
    JsonSerializerOptions GetSerializerOptions();
}