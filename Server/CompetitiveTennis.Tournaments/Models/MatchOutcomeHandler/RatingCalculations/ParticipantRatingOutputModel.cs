namespace CompetitiveTennis.Tournaments.Models.MatchOutcomeHandler.RatingCalculations;

public record ParticipantRatingOutputModel(
    int Id,
    bool IsGuest,
    string Specifier,
    IEnumerable<AccountRatingOutputModel> Players);