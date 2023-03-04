﻿namespace CompetitiveTennis.Tournaments.Models.Participant;

public record MultiParticipantInputModel(ParticipantInputModel ParticipantInfo, IEnumerable<int> Accounts, bool IncludeCurrentUser);