namespace CompetitiveTennis.Tournaments.Data;

public class DataConstants
{
    public const int MaxTeamNameLength = 75;
    public const int MaxScorePointsValueLength = 2;

    public class Tournaments
    {
        public const int MinTitleLength = 3;
        public const int MaxTitleLength = 100;
        public const int MinRulesLength = 45;
        public const int MaxRulesLength = 1500;
        public const int MinDescriptionLength = 10;
    }

    public class Avenues
    {
        public const int MinNameLength = 3;
        public const int MaxNameLength = 100;
        public const int MinLocationLength = 3;
        public const int MaxLocationLength = 250;
        public const int MaxDetailsLength = 1000;
        public const int MinCityCountryLength = 2;
    }

    public class Accounts
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 40;
    }
    public class CustomDbTypes
    {
        public const string TournamentTypeEnum = "tournament-type";
        public const string TournamentStageEnum = "tournament-stage";
        public const string SurfaceEnum = "surface";
        public const string EventStatusEnum = "event-status";
        public const string MatchOutcomeEnum = "match-outcome";
    }
}