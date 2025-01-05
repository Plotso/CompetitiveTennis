namespace CompetitiveTennis.Tournaments.Models.MatchOutcomeHandler;

using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Tournaments.Contracts.MatchPeriod;

public record MatchPeriodShortInfo(short Set, short Game, EventStatus Status, MatchOutcome Winner)
{
    public static MatchPeriodShortInfo FromMatchPeriodInput(MatchPeriodInputModel matchPeriodInputModel) 
        => new MatchPeriodShortInfo(matchPeriodInputModel.Set, matchPeriodInputModel.Game, matchPeriodInputModel.Status, matchPeriodInputModel.Winner);
    public static MatchPeriodShortInfo FromMatchPeriodInput(MatchPeriodOutputModel matchPeriodOutputModel) 
        => new MatchPeriodShortInfo(matchPeriodOutputModel.Set, matchPeriodOutputModel.Game, matchPeriodOutputModel.Status, matchPeriodOutputModel.Winner);
}