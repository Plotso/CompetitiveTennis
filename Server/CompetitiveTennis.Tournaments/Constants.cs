namespace CompetitiveTennis.Tournaments;

public class Constants
{
    public const int DefaultPlayerRating = 1000;
    
    public const int MaxTournamentTitleLength = 100;
    public const int MaxTournamentRulesLength = 1500;

    public const int MaxAvenueNameLength = 100;
    public const int MaxAvenueLocationLength = 250;
    public const int MaxAvenueDetailsLength = 1000;

    public const int MaxTeamNameLength = 75;

    public const int MaxScorePointsValueLength = 2;

    public class CustomDbTypes
    {
        public const string TournamentTypeEnum = "tournament-type";
        public const string SurfaceEnum = "surface";
        public const string EventStatusEnum = "event-status";
        public const string MatchOutcomeEnum = "match-outcome";
    }
}