﻿namespace CompetitiveTennis.Tournaments.Contracts;

public record SearchOutputModel<TRecord>(IEnumerable<TRecord> Results, int Page, int Total);