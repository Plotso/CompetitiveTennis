﻿namespace CompetitiveTennis.Tournaments.Models.Tournament;

using CompetitiveTennis.Data.Models.Enums;
using CompetitiveTennis.Models;

public record TournamentQuery(
    string Keyword = "", bool? HasEntryFee = null, bool? HasPrize = null,
    SortOptions SortOptions = SortOptions.CreatedAscending, Surface? Surface = null, TournamentType? TournamentType = null,
    bool? IsIndoor = null, DateTime? DateRangeFrom = null, DateTime? DateRangeUntil = null,
    int? OrganiserId = null, int[]? ParticipantIds = null, int Page = 1, int ItemsPerPage = 25) : PageQuery(Page, ItemsPerPage);