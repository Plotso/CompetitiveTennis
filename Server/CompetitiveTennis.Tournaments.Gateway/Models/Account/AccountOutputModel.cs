﻿namespace CompetitiveTennis.Tournaments.Gateway.Models.Account;

using Participant;
using Tournament;

public record AccountOutputModel(
    int Id,
    string Username,
    string FirstName,
    string LastName,
    int PlayerRating,
    IEnumerable<ParticipantShortOutputModel> Participations,
    IEnumerable<TournamentShortInfoOutput> OrganisedTournaments);