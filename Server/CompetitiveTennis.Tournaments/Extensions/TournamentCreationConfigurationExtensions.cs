namespace CompetitiveTennis.Tournaments.Extensions;

using Configurations;using Data.Configurations;

public static class TournamentCreationConfigurationExtensions
{
    public static bool HasValidDailyStartHour(this TournamentCreationConfiguration configuration)
        => IsValidHour(configuration.DailyStartHour);
    public static bool HasValidDailyEndHour(this TournamentCreationConfiguration configuration)
        => IsValidHour(configuration.DailyEndHour);
    
    private static bool IsValidHour(short hour) => hour is >= 0 and <= 24;
}