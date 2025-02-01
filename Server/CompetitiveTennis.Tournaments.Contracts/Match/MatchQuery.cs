namespace CompetitiveTennis.Tournaments.Contracts.Match;

using CompetitiveTennis.Data.Models.Enums;
using Models;

public record MatchQuery(
    string ParticipantUsername = "", string ParticipantName = "", bool? IsParticipantWinner = null,
    EventStatus? Status = null, OutcomeCondition? OutcomeCondition = null, TournamentType? TournamentType = null,
    DateTime? DateRangeFrom = null, DateTime? DateRangeUntil = null,
    SortOptions SortOptions = SortOptions.CreatedAscending, Surface? Surface = null, TournamentStage? TournamentStage = null,
    int Page = 1, int ItemsPerPage = 25) : PageQuery(Page, ItemsPerPage);