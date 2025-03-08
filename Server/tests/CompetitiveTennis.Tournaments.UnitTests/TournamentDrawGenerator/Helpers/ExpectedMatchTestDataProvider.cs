﻿namespace CompetitiveTennis.Tournaments.UnitTests.TournamentDrawGenerator.Helpers;

using CompetitiveTennis.Data.Models.Enums;
using Models.TournamentDrawGenerator;

public static class ExpectedMatchTestDataProvider
{
    public static MatchGeneratorOutput GetExpectedMatchBasedOnMatchNumberAndNumberOfSeeds(int matchNumber,
        int numberOfSeeds)
        => numberOfSeeds switch
        {
            2 => GetExpectedMatchFor2Participants(matchNumber),
            3 => GetExpectedMatchFor3Participants(matchNumber),
            4 => GetExpectedMatchFor4Participants(matchNumber),
            5 => GetExpectedMatchFor5Participants(matchNumber),
            6 => GetExpectedMatchFor6Participants(matchNumber),
            7 => GetExpectedMatchFor7Participants(matchNumber),
            8 => GetExpectedMatchFor8Participants(matchNumber),
            9 => GetExpectedMatchFor9Participants(matchNumber),
            10 => GetExpectedMatchFor10Participants(matchNumber),
            11 => GetExpectedMatchFor11Participants(matchNumber),
            12 => GetExpectedMatchFor12Participants(matchNumber),
            13 => GetExpectedMatchFor13Participants(matchNumber),
            31 => GetExpectedMatchFor31Participants(matchNumber),
            32 => GetExpectedMatchFor32Participants(matchNumber),
            36 => GetExpectedMatchFor36Participants(matchNumber),
            40 => GetExpectedMatchFor40Participants(matchNumber),
            41 => GetExpectedMatchFor41Participants(matchNumber),
            42 => GetExpectedMatchFor42Participants(matchNumber),
            84 => GetExpectedMatchFor84Participants(matchNumber),
            86 => GetExpectedMatchFor86Participants(matchNumber),
            100 => GetExpectedMatchFor100Participants(matchNumber),
            _ => throw new ArgumentOutOfRangeException(
                $"No method configure to support expected match gathering for {numberOfSeeds} seeds!")
        };

    private static MatchGeneratorOutput GetExpectedMatchFor2Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Seed 2", TournamentStage = TournamentStage.Final, PlayOrderNumber = 1,
            HomePrevMatch = null, AwayPrevMatch = null
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor3Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Seed 3", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = 1
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor4Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Seed 4", TournamentStage = TournamentStage.SemiFinal,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Seed 3", TournamentStage = TournamentStage.SemiFinal,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 1", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 3, HomePrevMatch = 1, AwayPrevMatch = 2
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor5Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 5", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.SemiFinal,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = 1
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Seed 3", TournamentStage = TournamentStage.SemiFinal,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 2", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 4, HomePrevMatch = 2, AwayPrevMatch = 3
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor6Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 5", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Seed 6", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.SemiFinal,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = 1
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.SemiFinal,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = 2
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 3", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 5, HomePrevMatch = 3, AwayPrevMatch = 4
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor7Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 5", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Seed 7", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Seed 6", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.SemiFinal,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = 1
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 2", AwayPlayer = "Winner of match 3",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 5, HomePrevMatch = 2, AwayPrevMatch = 3
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 4", AwayPlayer = "Winner of match 5", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 6, HomePrevMatch = 4, AwayPrevMatch = 5
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor8Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Seed 8", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 5", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Seed 7", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Seed 6", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 1", AwayPlayer = "Winner of match 2",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 5, HomePrevMatch = 1, AwayPrevMatch = 2
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 3", AwayPlayer = "Winner of match 4",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 6, HomePrevMatch = 3, AwayPrevMatch = 4
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 5", AwayPlayer = "Winner of match 6", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 7, HomePrevMatch = 5, AwayPrevMatch = 6
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor9Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Seed 9", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = 1
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 5", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Seed 7", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Seed 6", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 2", AwayPlayer = "Winner of match 3",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 6, HomePrevMatch = 2, AwayPrevMatch = 3
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 4", AwayPlayer = "Winner of match 5",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 7, HomePrevMatch = 4, AwayPrevMatch = 5
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 6", AwayPlayer = "Winner of match 7", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 8, HomePrevMatch = 6, AwayPrevMatch = 7
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor10Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Seed 9", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Seed 10", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = 1
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 5", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = 2
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Seed 6", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 3", AwayPlayer = "Winner of match 4",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 7, HomePrevMatch = 3, AwayPrevMatch = 4
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 5", AwayPlayer = "Winner of match 6",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 8, HomePrevMatch = 5, AwayPrevMatch = 6
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 7", AwayPlayer = "Winner of match 8", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 9, HomePrevMatch = 7, AwayPrevMatch = 8
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor11Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Seed 9", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Seed 10", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Seed 11", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = 1
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 5", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = 2
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = 3
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 4", AwayPlayer = "Winner of match 5",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 8, HomePrevMatch = 4, AwayPrevMatch = 5
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 6", AwayPlayer = "Winner of match 7",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 9, HomePrevMatch = 6, AwayPrevMatch = 7
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 8", AwayPlayer = "Winner of match 9", TournamentStage = TournamentStage.Final,
            PlayOrderNumber = 10, HomePrevMatch = 8, AwayPrevMatch = 9
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor12Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Seed 9", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Seed 12", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Seed 10", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Seed 11", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = 1
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = 2
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = 3
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = 4
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 5", AwayPlayer = "Winner of match 6",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 9, HomePrevMatch = 5, AwayPrevMatch = 6
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 7", AwayPlayer = "Winner of match 8",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 10, HomePrevMatch = 7, AwayPrevMatch = 8
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 9", AwayPlayer = "Winner of match 10",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 11, HomePrevMatch = 9, AwayPrevMatch = 10
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor13Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Seed 9", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 13", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Seed 12", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Seed 10", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Seed 11", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = 1
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 2", AwayPlayer = "Winner of match 3",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 7, HomePrevMatch = 2, AwayPrevMatch = 3
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = 4
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 5", TournamentStage = TournamentStage.QuarterFinal,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = 5
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 6", AwayPlayer = "Winner of match 7",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 10, HomePrevMatch = 6, AwayPrevMatch = 7
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 8", AwayPlayer = "Winner of match 9",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 11, HomePrevMatch = 8, AwayPrevMatch = 9
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 10", AwayPlayer = "Winner of match 11",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 12, HomePrevMatch = 10, AwayPrevMatch = 11
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor32Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Seed 32", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Seed 17", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Seed 25", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Seed 24", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 29", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Seed 20", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Seed 28", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Seed 21", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Seed 31", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Seed 18", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Seed 26", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Seed 23", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Seed 30", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Seed 19", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Seed 27", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Seed 22", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = null
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 1", AwayPlayer = "Winner of match 2",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 17, HomePrevMatch = 1, AwayPrevMatch = 2
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 3", AwayPlayer = "Winner of match 4",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 18, HomePrevMatch = 3, AwayPrevMatch = 4
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 5", AwayPlayer = "Winner of match 6",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 19, HomePrevMatch = 5, AwayPrevMatch = 6
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 7", AwayPlayer = "Winner of match 8",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 20, HomePrevMatch = 7, AwayPrevMatch = 8
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 9", AwayPlayer = "Winner of match 10",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 21, HomePrevMatch = 9, AwayPrevMatch = 10
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 11", AwayPlayer = "Winner of match 12",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 22, HomePrevMatch = 11, AwayPrevMatch = 12
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 13", AwayPlayer = "Winner of match 14",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 23, HomePrevMatch = 13, AwayPrevMatch = 14
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 15", AwayPlayer = "Winner of match 16",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 24, HomePrevMatch = 15, AwayPrevMatch = 16
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 17", AwayPlayer = "Winner of match 18",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 25, HomePrevMatch = 17, AwayPrevMatch = 18
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 19", AwayPlayer = "Winner of match 20",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 26, HomePrevMatch = 19, AwayPrevMatch = 20
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 21", AwayPlayer = "Winner of match 22",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 27, HomePrevMatch = 21, AwayPrevMatch = 22
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 23", AwayPlayer = "Winner of match 24",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 28, HomePrevMatch = 23, AwayPrevMatch = 24
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 25", AwayPlayer = "Winner of match 26",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 29, HomePrevMatch = 25, AwayPrevMatch = 26
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 27", AwayPlayer = "Winner of match 28",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 30, HomePrevMatch = 27, AwayPrevMatch = 28
        },
        31 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 29", AwayPlayer = "Winner of match 30",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 31, HomePrevMatch = 29, AwayPrevMatch = 30
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor31Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Seed 17", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Seed 25", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Seed 24", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Seed 29", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Seed 20", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Seed 28", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Seed 21", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Seed 31", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Seed 18", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Seed 26", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Seed 23", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Seed 30", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Seed 19", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Seed 27", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Seed 22", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.RoundOf16,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = 1
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 2", AwayPlayer = "Winner of match 3",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 17, HomePrevMatch = 2, AwayPrevMatch = 3
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 4", AwayPlayer = "Winner of match 5",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 18, HomePrevMatch = 4, AwayPrevMatch = 5
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 6", AwayPlayer = "Winner of match 7",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 19, HomePrevMatch = 6, AwayPrevMatch = 7
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 8", AwayPlayer = "Winner of match 9",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 20, HomePrevMatch = 8, AwayPrevMatch = 9
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 10", AwayPlayer = "Winner of match 11",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 21, HomePrevMatch = 10, AwayPrevMatch = 11
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 12", AwayPlayer = "Winner of match 13",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 22, HomePrevMatch = 12, AwayPrevMatch = 13
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 14", AwayPlayer = "Winner of match 15",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 23, HomePrevMatch = 14, AwayPrevMatch = 15
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 16", AwayPlayer = "Winner of match 17",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 24, HomePrevMatch = 16, AwayPrevMatch = 17
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 18", AwayPlayer = "Winner of match 19",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 25, HomePrevMatch = 18, AwayPrevMatch = 19
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 20", AwayPlayer = "Winner of match 21",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 26, HomePrevMatch = 20, AwayPrevMatch = 21
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 22", AwayPlayer = "Winner of match 23",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 27, HomePrevMatch = 22, AwayPrevMatch = 23
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 24", AwayPlayer = "Winner of match 25",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 28, HomePrevMatch = 24, AwayPrevMatch = 25
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 26", AwayPlayer = "Winner of match 27",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 29, HomePrevMatch = 26, AwayPrevMatch = 27
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 28", AwayPlayer = "Winner of match 29",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 30, HomePrevMatch = 28, AwayPrevMatch = 29
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor41Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 32", AwayPlayer = "Seed 33", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 25", AwayPlayer = "Seed 40", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 24", AwayPlayer = "Seed 41", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 29", AwayPlayer = "Seed 36", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 28", AwayPlayer = "Seed 37", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 31", AwayPlayer = "Seed 34", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 26", AwayPlayer = "Seed 39", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 30", AwayPlayer = "Seed 35", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 27", AwayPlayer = "Seed 38", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 17, HomePrevMatch = null, AwayPrevMatch = 1
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Seed 17", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 18, HomePrevMatch = null, AwayPrevMatch = 2
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 19, HomePrevMatch = null, AwayPrevMatch = 3
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 20, HomePrevMatch = null, AwayPrevMatch = 4
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Seed 20", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Winner of match 5", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 21, HomePrevMatch = null, AwayPrevMatch = 5
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Seed 21", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 6", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 22, HomePrevMatch = null, AwayPrevMatch = 6
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Seed 18", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Winner of match 7", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 23, HomePrevMatch = null, AwayPrevMatch = 7
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Seed 23", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 8", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 24, HomePrevMatch = null, AwayPrevMatch = 8
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Seed 19", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Winner of match 9", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 25, HomePrevMatch = null, AwayPrevMatch = 9
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Seed 22", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = null
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 10", AwayPlayer = "Winner of match 11",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 26, HomePrevMatch = 10, AwayPrevMatch = 11
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 12", AwayPlayer = "Winner of match 13",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 27, HomePrevMatch = 12, AwayPrevMatch = 13
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 14", AwayPlayer = "Winner of match 15",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 28, HomePrevMatch = 14, AwayPrevMatch = 15
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 16", AwayPlayer = "Winner of match 17",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 29, HomePrevMatch = 16, AwayPrevMatch = 17
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 18", AwayPlayer = "Winner of match 19",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 30, HomePrevMatch = 18, AwayPrevMatch = 19
        },
        31 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 20", AwayPlayer = "Winner of match 21",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 31, HomePrevMatch = 20, AwayPrevMatch = 21
        },
        32 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 22", AwayPlayer = "Winner of match 23",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 32, HomePrevMatch = 22, AwayPrevMatch = 23
        },
        33 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 24", AwayPlayer = "Winner of match 25",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 33, HomePrevMatch = 24, AwayPrevMatch = 25
        },
        34 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 26", AwayPlayer = "Winner of match 27",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 34, HomePrevMatch = 26, AwayPrevMatch = 27
        },
        35 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 28", AwayPlayer = "Winner of match 29",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 35, HomePrevMatch = 28, AwayPrevMatch = 29
        },
        36 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 30", AwayPlayer = "Winner of match 31",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 36, HomePrevMatch = 30, AwayPrevMatch = 31
        },
        37 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 32", AwayPlayer = "Winner of match 33",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 37, HomePrevMatch = 32, AwayPrevMatch = 33
        },
        38 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 34", AwayPlayer = "Winner of match 35",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 38, HomePrevMatch = 34, AwayPrevMatch = 35
        },
        39 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 36", AwayPlayer = "Winner of match 37",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 39, HomePrevMatch = 36, AwayPrevMatch = 37
        },
        40 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 38", AwayPlayer = "Winner of match 39",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 40, HomePrevMatch = 38, AwayPrevMatch = 39
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor42Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 32", AwayPlayer = "Seed 33", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 25", AwayPlayer = "Seed 40", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 24", AwayPlayer = "Seed 41", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 29", AwayPlayer = "Seed 36", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 28", AwayPlayer = "Seed 37", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 31", AwayPlayer = "Seed 34", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 26", AwayPlayer = "Seed 39", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 23", AwayPlayer = "Seed 42", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 30", AwayPlayer = "Seed 35", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 27", AwayPlayer = "Seed 38", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 17, HomePrevMatch = null, AwayPrevMatch = 1
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Seed 17", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 18, HomePrevMatch = null, AwayPrevMatch = 2
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 19, HomePrevMatch = null, AwayPrevMatch = 3
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 20, HomePrevMatch = null, AwayPrevMatch = 4
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Seed 20", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Winner of match 5", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 21, HomePrevMatch = null, AwayPrevMatch = 5
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Seed 21", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 6", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 22, HomePrevMatch = null, AwayPrevMatch = 6
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Seed 18", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Winner of match 7", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 23, HomePrevMatch = null, AwayPrevMatch = 7
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Winner of match 8", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 24, HomePrevMatch = null, AwayPrevMatch = 8
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 9", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 25, HomePrevMatch = null, AwayPrevMatch = 9
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Seed 19", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Winner of match 10", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 26, HomePrevMatch = null, AwayPrevMatch = 10
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Seed 22", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = null
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 11", AwayPlayer = "Winner of match 12",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 27, HomePrevMatch = 11, AwayPrevMatch = 12
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 13", AwayPlayer = "Winner of match 14",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 28, HomePrevMatch = 13, AwayPrevMatch = 14
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 15", AwayPlayer = "Winner of match 16",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 29, HomePrevMatch = 15, AwayPrevMatch = 16
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 17", AwayPlayer = "Winner of match 18",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 30, HomePrevMatch = 17, AwayPrevMatch = 18
        },
        31 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 19", AwayPlayer = "Winner of match 20",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 31, HomePrevMatch = 19, AwayPrevMatch = 20
        },
        32 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 21", AwayPlayer = "Winner of match 22",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 32, HomePrevMatch = 21, AwayPrevMatch = 22
        },
        33 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 23", AwayPlayer = "Winner of match 24",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 33, HomePrevMatch = 23, AwayPrevMatch = 24
        },
        34 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 25", AwayPlayer = "Winner of match 26",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 34, HomePrevMatch = 25, AwayPrevMatch = 26
        },
        35 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 27", AwayPlayer = "Winner of match 28",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 35, HomePrevMatch = 27, AwayPrevMatch = 28
        },
        36 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 29", AwayPlayer = "Winner of match 30",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 36, HomePrevMatch = 29, AwayPrevMatch = 30
        },
        37 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 31", AwayPlayer = "Winner of match 32",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 37, HomePrevMatch = 31, AwayPrevMatch = 32
        },
        38 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 33", AwayPlayer = "Winner of match 34",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 38, HomePrevMatch = 33, AwayPrevMatch = 34
        },
        39 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 35", AwayPlayer = "Winner of match 36",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 39, HomePrevMatch = 35, AwayPrevMatch = 36
        },
        40 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 37", AwayPlayer = "Winner of match 38",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 40, HomePrevMatch = 37, AwayPrevMatch = 38
        },
        41 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 39", AwayPlayer = "Winner of match 40",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 41, HomePrevMatch = 39, AwayPrevMatch = 40
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor84Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 64", AwayPlayer = "Seed 65", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 49", AwayPlayer = "Seed 80", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 48", AwayPlayer = "Seed 81", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 57", AwayPlayer = "Seed 72", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 56", AwayPlayer = "Seed 73", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 61", AwayPlayer = "Seed 68", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 52", AwayPlayer = "Seed 77", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 45", AwayPlayer = "Seed 84", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 60", AwayPlayer = "Seed 69", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 53", AwayPlayer = "Seed 76", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 63", AwayPlayer = "Seed 66", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 50", AwayPlayer = "Seed 79", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 47", AwayPlayer = "Seed 82", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 58", AwayPlayer = "Seed 71", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 55", AwayPlayer = "Seed 74", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 62", AwayPlayer = "Seed 67", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = null
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 51", AwayPlayer = "Seed 78", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 17, HomePrevMatch = null, AwayPrevMatch = null
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 46", AwayPlayer = "Seed 83", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 18, HomePrevMatch = null, AwayPrevMatch = null
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 59", AwayPlayer = "Seed 70", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 19, HomePrevMatch = null, AwayPrevMatch = null
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 54", AwayPlayer = "Seed 75", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 20, HomePrevMatch = null, AwayPrevMatch = null
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 33, HomePrevMatch = null, AwayPrevMatch = 1
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 32", AwayPlayer = "Seed 33", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 21, HomePrevMatch = null, AwayPrevMatch = null
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 34, HomePrevMatch = null, AwayPrevMatch = 2
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 17", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 35, HomePrevMatch = null, AwayPrevMatch = 3
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 36, HomePrevMatch = null, AwayPrevMatch = 4
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 25", AwayPlayer = "Seed 40", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 22, HomePrevMatch = null, AwayPrevMatch = null
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Winner of match 5", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 37, HomePrevMatch = null, AwayPrevMatch = 5
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 24", AwayPlayer = "Seed 41", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 23, HomePrevMatch = null, AwayPrevMatch = null
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Winner of match 6", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 38, HomePrevMatch = null, AwayPrevMatch = 6
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 29", AwayPlayer = "Seed 36", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 24, HomePrevMatch = null, AwayPrevMatch = null
        },
        31 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Winner of match 7", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 39, HomePrevMatch = null, AwayPrevMatch = 7
        },
        32 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 20", AwayPlayer = "Winner of match 8", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 40, HomePrevMatch = null, AwayPrevMatch = 8
        },
        33 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Winner of match 9", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 41, HomePrevMatch = null, AwayPrevMatch = 9
        },
        34 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 28", AwayPlayer = "Seed 37", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 25, HomePrevMatch = null, AwayPrevMatch = null
        },
        35 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Winner of match 10", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 42, HomePrevMatch = null, AwayPrevMatch = 10
        },
        36 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 21", AwayPlayer = "Seed 44", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 26, HomePrevMatch = null, AwayPrevMatch = null
        },
        37 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 11", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 43, HomePrevMatch = null, AwayPrevMatch = 11
        },
        38 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 31", AwayPlayer = "Seed 34", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 27, HomePrevMatch = null, AwayPrevMatch = null
        },
        39 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Winner of match 12", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 44, HomePrevMatch = null, AwayPrevMatch = 12
        },
        40 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 18", AwayPlayer = "Winner of match 13", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 45, HomePrevMatch = null, AwayPrevMatch = 13
        },
        41 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Winner of match 14", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 46, HomePrevMatch = null, AwayPrevMatch = 14
        },
        42 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 26", AwayPlayer = "Seed 39", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 28, HomePrevMatch = null, AwayPrevMatch = null
        },
        43 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Winner of match 15", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 47, HomePrevMatch = null, AwayPrevMatch = 15
        },
        44 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 23", AwayPlayer = "Seed 42", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 29, HomePrevMatch = null, AwayPrevMatch = null
        },
        45 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 16", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 48, HomePrevMatch = null, AwayPrevMatch = 16
        },
        46 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 30", AwayPlayer = "Seed 35", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 30, HomePrevMatch = null, AwayPrevMatch = null
        },
        47 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Winner of match 17", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 49, HomePrevMatch = null, AwayPrevMatch = 17
        },
        48 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 19", AwayPlayer = "Winner of match 18", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 50, HomePrevMatch = null, AwayPrevMatch = 18
        },
        49 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Winner of match 19", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 51, HomePrevMatch = null, AwayPrevMatch = 19
        },
        50 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 27", AwayPlayer = "Seed 38", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 31, HomePrevMatch = null, AwayPrevMatch = null
        },
        51 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Winner of match 20", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 52, HomePrevMatch = null, AwayPrevMatch = 20
        },
        52 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 22", AwayPlayer = "Seed 43", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 32, HomePrevMatch = null, AwayPrevMatch = null
        },
        53 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 21", AwayPlayer = "Winner of match 22",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 53, HomePrevMatch = 21, AwayPrevMatch = 22
        },
        54 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 23", AwayPlayer = "Winner of match 24",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 54, HomePrevMatch = 23, AwayPrevMatch = 24
        },
        55 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 25", AwayPlayer = "Winner of match 26",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 55, HomePrevMatch = 25, AwayPrevMatch = 26
        },
        56 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 27", AwayPlayer = "Winner of match 28",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 56, HomePrevMatch = 27, AwayPrevMatch = 28
        },
        57 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 29", AwayPlayer = "Winner of match 30",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 57, HomePrevMatch = 29, AwayPrevMatch = 30
        },
        58 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 31", AwayPlayer = "Winner of match 32",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 58, HomePrevMatch = 31, AwayPrevMatch = 32
        },
        59 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 33", AwayPlayer = "Winner of match 34",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 59, HomePrevMatch = 33, AwayPrevMatch = 34
        },
        60 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 35", AwayPlayer = "Winner of match 36",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 60, HomePrevMatch = 35, AwayPrevMatch = 36
        },
        61 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 37", AwayPlayer = "Winner of match 38",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 61, HomePrevMatch = 37, AwayPrevMatch = 38
        },
        62 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 39", AwayPlayer = "Winner of match 40",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 62, HomePrevMatch = 39, AwayPrevMatch = 40
        },
        63 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 41", AwayPlayer = "Winner of match 42",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 63, HomePrevMatch = 41, AwayPrevMatch = 42
        },
        64 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 43", AwayPlayer = "Winner of match 44",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 64, HomePrevMatch = 43, AwayPrevMatch = 44
        },
        65 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 45", AwayPlayer = "Winner of match 46",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 65, HomePrevMatch = 45, AwayPrevMatch = 46
        },
        66 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 47", AwayPlayer = "Winner of match 48",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 66, HomePrevMatch = 47, AwayPrevMatch = 48
        },
        67 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 49", AwayPlayer = "Winner of match 50",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 67, HomePrevMatch = 49, AwayPrevMatch = 50
        },
        68 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 51", AwayPlayer = "Winner of match 52",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 68, HomePrevMatch = 51, AwayPrevMatch = 52
        },
        69 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 53", AwayPlayer = "Winner of match 54",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 69, HomePrevMatch = 53, AwayPrevMatch = 54
        },
        70 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 55", AwayPlayer = "Winner of match 56",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 70, HomePrevMatch = 55, AwayPrevMatch = 56
        },
        71 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 57", AwayPlayer = "Winner of match 58",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 71, HomePrevMatch = 57, AwayPrevMatch = 58
        },
        72 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 59", AwayPlayer = "Winner of match 60",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 72, HomePrevMatch = 59, AwayPrevMatch = 60
        },
        73 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 61", AwayPlayer = "Winner of match 62",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 73, HomePrevMatch = 61, AwayPrevMatch = 62
        },
        74 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 63", AwayPlayer = "Winner of match 64",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 74, HomePrevMatch = 63, AwayPrevMatch = 64
        },
        75 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 65", AwayPlayer = "Winner of match 66",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 75, HomePrevMatch = 65, AwayPrevMatch = 66
        },
        76 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 67", AwayPlayer = "Winner of match 68",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 76, HomePrevMatch = 67, AwayPrevMatch = 68
        },
        77 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 69", AwayPlayer = "Winner of match 70",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 77, HomePrevMatch = 69, AwayPrevMatch = 70
        },
        78 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 71", AwayPlayer = "Winner of match 72",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 78, HomePrevMatch = 71, AwayPrevMatch = 72
        },
        79 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 73", AwayPlayer = "Winner of match 74",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 79, HomePrevMatch = 73, AwayPrevMatch = 74
        },
        80 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 75", AwayPlayer = "Winner of match 76",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 80, HomePrevMatch = 75, AwayPrevMatch = 76
        },
        81 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 77", AwayPlayer = "Winner of match 78",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 81, HomePrevMatch = 77, AwayPrevMatch = 78
        },
        82 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 79", AwayPlayer = "Winner of match 80",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 82, HomePrevMatch = 79, AwayPrevMatch = 80
        },
        83 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 81", AwayPlayer = "Winner of match 82",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 83, HomePrevMatch = 81, AwayPrevMatch = 82
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor86Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 64", AwayPlayer = "Seed 65", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 49", AwayPlayer = "Seed 80", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 48", AwayPlayer = "Seed 81", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 57", AwayPlayer = "Seed 72", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 56", AwayPlayer = "Seed 73", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 61", AwayPlayer = "Seed 68", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 52", AwayPlayer = "Seed 77", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 45", AwayPlayer = "Seed 84", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 60", AwayPlayer = "Seed 69", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 53", AwayPlayer = "Seed 76", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 44", AwayPlayer = "Seed 85", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 63", AwayPlayer = "Seed 66", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 50", AwayPlayer = "Seed 79", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 47", AwayPlayer = "Seed 82", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 58", AwayPlayer = "Seed 71", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 55", AwayPlayer = "Seed 74", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = null
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 62", AwayPlayer = "Seed 67", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 17, HomePrevMatch = null, AwayPrevMatch = null
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 51", AwayPlayer = "Seed 78", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 18, HomePrevMatch = null, AwayPrevMatch = null
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 46", AwayPlayer = "Seed 83", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 19, HomePrevMatch = null, AwayPrevMatch = null
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 59", AwayPlayer = "Seed 70", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 20, HomePrevMatch = null, AwayPrevMatch = null
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 54", AwayPlayer = "Seed 75", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 21, HomePrevMatch = null, AwayPrevMatch = null
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 43", AwayPlayer = "Seed 86", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 22, HomePrevMatch = null, AwayPrevMatch = null
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 33, HomePrevMatch = null, AwayPrevMatch = 1
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 32", AwayPlayer = "Seed 33", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 23, HomePrevMatch = null, AwayPrevMatch = null
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 34, HomePrevMatch = null, AwayPrevMatch = 2
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 17", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 35, HomePrevMatch = null, AwayPrevMatch = 3
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 36, HomePrevMatch = null, AwayPrevMatch = 4
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 25", AwayPlayer = "Seed 40", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 24, HomePrevMatch = null, AwayPrevMatch = null
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Winner of match 5", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 37, HomePrevMatch = null, AwayPrevMatch = 5
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 24", AwayPlayer = "Seed 41", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 25, HomePrevMatch = null, AwayPrevMatch = null
        },
        31 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Winner of match 6", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 38, HomePrevMatch = null, AwayPrevMatch = 6
        },
        32 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 29", AwayPlayer = "Seed 36", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 26, HomePrevMatch = null, AwayPrevMatch = null
        },
        33 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Winner of match 7", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 39, HomePrevMatch = null, AwayPrevMatch = 7
        },
        34 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 20", AwayPlayer = "Winner of match 8", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 40, HomePrevMatch = null, AwayPrevMatch = 8
        },
        35 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Winner of match 9", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 41, HomePrevMatch = null, AwayPrevMatch = 9
        },
        36 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 28", AwayPlayer = "Seed 37", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 27, HomePrevMatch = null, AwayPrevMatch = null
        },
        37 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Winner of match 10", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 42, HomePrevMatch = null, AwayPrevMatch = 10
        },
        38 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 21", AwayPlayer = "Winner of match 11", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 43, HomePrevMatch = null, AwayPrevMatch = 11
        },
        39 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 12", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 44, HomePrevMatch = null, AwayPrevMatch = 12
        },
        40 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 31", AwayPlayer = "Seed 34", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 28, HomePrevMatch = null, AwayPrevMatch = null
        },
        41 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Winner of match 13", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 45, HomePrevMatch = null, AwayPrevMatch = 13
        },
        42 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 18", AwayPlayer = "Winner of match 14", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 46, HomePrevMatch = null, AwayPrevMatch = 14
        },
        43 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Winner of match 15", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 47, HomePrevMatch = null, AwayPrevMatch = 15
        },
        44 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 26", AwayPlayer = "Seed 39", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 29, HomePrevMatch = null, AwayPrevMatch = null
        },
        45 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Winner of match 16", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 48, HomePrevMatch = null, AwayPrevMatch = 16
        },
        46 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 23", AwayPlayer = "Seed 42", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 30, HomePrevMatch = null, AwayPrevMatch = null
        },
        47 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 17", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 49, HomePrevMatch = null, AwayPrevMatch = 17
        },
        48 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 30", AwayPlayer = "Seed 35", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 31, HomePrevMatch = null, AwayPrevMatch = null
        },
        49 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Winner of match 18", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 50, HomePrevMatch = null, AwayPrevMatch = 18
        },
        50 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 19", AwayPlayer = "Winner of match 19", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 51, HomePrevMatch = null, AwayPrevMatch = 19
        },
        51 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Winner of match 20", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 52, HomePrevMatch = null, AwayPrevMatch = 20
        },
        52 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 27", AwayPlayer = "Seed 38", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 32, HomePrevMatch = null, AwayPrevMatch = null
        },
        53 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Winner of match 21", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 53, HomePrevMatch = null, AwayPrevMatch = 21
        },
        54 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 22", AwayPlayer = "Winner of match 22", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 54, HomePrevMatch = null, AwayPrevMatch = 22
        },
        55 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 23", AwayPlayer = "Winner of match 24",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 55, HomePrevMatch = 23, AwayPrevMatch = 24
        },
        56 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 25", AwayPlayer = "Winner of match 26",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 56, HomePrevMatch = 25, AwayPrevMatch = 26
        },
        57 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 27", AwayPlayer = "Winner of match 28",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 57, HomePrevMatch = 27, AwayPrevMatch = 28
        },
        58 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 29", AwayPlayer = "Winner of match 30",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 58, HomePrevMatch = 29, AwayPrevMatch = 30
        },
        59 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 31", AwayPlayer = "Winner of match 32",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 59, HomePrevMatch = 31, AwayPrevMatch = 32
        },
        60 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 33", AwayPlayer = "Winner of match 34",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 60, HomePrevMatch = 33, AwayPrevMatch = 34
        },
        61 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 35", AwayPlayer = "Winner of match 36",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 61, HomePrevMatch = 35, AwayPrevMatch = 36
        },
        62 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 37", AwayPlayer = "Winner of match 38",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 62, HomePrevMatch = 37, AwayPrevMatch = 38
        },
        63 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 39", AwayPlayer = "Winner of match 40",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 63, HomePrevMatch = 39, AwayPrevMatch = 40
        },
        64 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 41", AwayPlayer = "Winner of match 42",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 64, HomePrevMatch = 41, AwayPrevMatch = 42
        },
        65 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 43", AwayPlayer = "Winner of match 44",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 65, HomePrevMatch = 43, AwayPrevMatch = 44
        },
        66 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 45", AwayPlayer = "Winner of match 46",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 66, HomePrevMatch = 45, AwayPrevMatch = 46
        },
        67 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 47", AwayPlayer = "Winner of match 48",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 67, HomePrevMatch = 47, AwayPrevMatch = 48
        },
        68 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 49", AwayPlayer = "Winner of match 50",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 68, HomePrevMatch = 49, AwayPrevMatch = 50
        },
        69 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 51", AwayPlayer = "Winner of match 52",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 69, HomePrevMatch = 51, AwayPrevMatch = 52
        },
        70 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 53", AwayPlayer = "Winner of match 54",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 70, HomePrevMatch = 53, AwayPrevMatch = 54
        },
        71 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 55", AwayPlayer = "Winner of match 56",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 71, HomePrevMatch = 55, AwayPrevMatch = 56
        },
        72 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 57", AwayPlayer = "Winner of match 58",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 72, HomePrevMatch = 57, AwayPrevMatch = 58
        },
        73 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 59", AwayPlayer = "Winner of match 60",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 73, HomePrevMatch = 59, AwayPrevMatch = 60
        },
        74 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 61", AwayPlayer = "Winner of match 62",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 74, HomePrevMatch = 61, AwayPrevMatch = 62
        },
        75 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 63", AwayPlayer = "Winner of match 64",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 75, HomePrevMatch = 63, AwayPrevMatch = 64
        },
        76 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 65", AwayPlayer = "Winner of match 66",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 76, HomePrevMatch = 65, AwayPrevMatch = 66
        },
        77 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 67", AwayPlayer = "Winner of match 68",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 77, HomePrevMatch = 67, AwayPrevMatch = 68
        },
        78 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 69", AwayPlayer = "Winner of match 70",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 78, HomePrevMatch = 69, AwayPrevMatch = 70
        },
        79 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 71", AwayPlayer = "Winner of match 72",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 79, HomePrevMatch = 71, AwayPrevMatch = 72
        },
        80 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 73", AwayPlayer = "Winner of match 74",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 80, HomePrevMatch = 73, AwayPrevMatch = 74
        },
        81 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 75", AwayPlayer = "Winner of match 76",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 81, HomePrevMatch = 75, AwayPrevMatch = 76
        },
        82 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 77", AwayPlayer = "Winner of match 78",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 82, HomePrevMatch = 77, AwayPrevMatch = 78
        },
        83 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 79", AwayPlayer = "Winner of match 80",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 83, HomePrevMatch = 79, AwayPrevMatch = 80
        },
        84 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 81", AwayPlayer = "Winner of match 82",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 84, HomePrevMatch = 81, AwayPrevMatch = 82
        },
        85 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 83", AwayPlayer = "Winner of match 84",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 85, HomePrevMatch = 83, AwayPrevMatch = 84
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor100Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 64", AwayPlayer = "Seed 65", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 32", AwayPlayer = "Seed 97", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 33", AwayPlayer = "Seed 96", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 49", AwayPlayer = "Seed 80", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 48", AwayPlayer = "Seed 81", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 57", AwayPlayer = "Seed 72", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 40", AwayPlayer = "Seed 89", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 56", AwayPlayer = "Seed 73", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 41", AwayPlayer = "Seed 88", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 61", AwayPlayer = "Seed 68", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 29", AwayPlayer = "Seed 100", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 36", AwayPlayer = "Seed 93", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 52", AwayPlayer = "Seed 77", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 45", AwayPlayer = "Seed 84", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 60", AwayPlayer = "Seed 69", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 37", AwayPlayer = "Seed 92", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = null
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 53", AwayPlayer = "Seed 76", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 17, HomePrevMatch = null, AwayPrevMatch = null
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 44", AwayPlayer = "Seed 85", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 18, HomePrevMatch = null, AwayPrevMatch = null
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 63", AwayPlayer = "Seed 66", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 19, HomePrevMatch = null, AwayPrevMatch = null
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 31", AwayPlayer = "Seed 98", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 20, HomePrevMatch = null, AwayPrevMatch = null
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 34", AwayPlayer = "Seed 95", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 21, HomePrevMatch = null, AwayPrevMatch = null
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 50", AwayPlayer = "Seed 79", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 22, HomePrevMatch = null, AwayPrevMatch = null
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 47", AwayPlayer = "Seed 82", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 23, HomePrevMatch = null, AwayPrevMatch = null
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 58", AwayPlayer = "Seed 71", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 24, HomePrevMatch = null, AwayPrevMatch = null
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 39", AwayPlayer = "Seed 90", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 25, HomePrevMatch = null, AwayPrevMatch = null
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 55", AwayPlayer = "Seed 74", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 26, HomePrevMatch = null, AwayPrevMatch = null
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 42", AwayPlayer = "Seed 87", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 27, HomePrevMatch = null, AwayPrevMatch = null
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 62", AwayPlayer = "Seed 67", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 28, HomePrevMatch = null, AwayPrevMatch = null
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 30", AwayPlayer = "Seed 99", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 29, HomePrevMatch = null, AwayPrevMatch = null
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 35", AwayPlayer = "Seed 94", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 30, HomePrevMatch = null, AwayPrevMatch = null
        },
        31 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 51", AwayPlayer = "Seed 78", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 31, HomePrevMatch = null, AwayPrevMatch = null
        },
        32 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 46", AwayPlayer = "Seed 83", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 32, HomePrevMatch = null, AwayPrevMatch = null
        },
        33 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 59", AwayPlayer = "Seed 70", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 33, HomePrevMatch = null, AwayPrevMatch = null
        },
        34 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 38", AwayPlayer = "Seed 91", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 34, HomePrevMatch = null, AwayPrevMatch = null
        },
        35 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 54", AwayPlayer = "Seed 75", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 35, HomePrevMatch = null, AwayPrevMatch = null
        },
        36 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 43", AwayPlayer = "Seed 86", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 36, HomePrevMatch = null, AwayPrevMatch = null
        },
        37 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 37, HomePrevMatch = null, AwayPrevMatch = 1
        },
        38 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 2", AwayPlayer = "Winner of match 3",
            TournamentStage = TournamentStage.RoundOf64, PlayOrderNumber = 38, HomePrevMatch = 2, AwayPrevMatch = 3
        },
        39 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 39, HomePrevMatch = null, AwayPrevMatch = 4
        },
        40 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 17", AwayPlayer = "Winner of match 5", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 40, HomePrevMatch = null, AwayPrevMatch = 5
        },
        41 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Winner of match 6", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 41, HomePrevMatch = null, AwayPrevMatch = 6
        },
        42 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 25", AwayPlayer = "Winner of match 7", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 42, HomePrevMatch = null, AwayPrevMatch = 7
        },
        43 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Winner of match 8", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 43, HomePrevMatch = null, AwayPrevMatch = 8
        },
        44 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 24", AwayPlayer = "Winner of match 9", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 44, HomePrevMatch = null, AwayPrevMatch = 9
        },
        45 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Winner of match 10", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 45, HomePrevMatch = null, AwayPrevMatch = 10
        },
        46 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 11", AwayPlayer = "Winner of match 12",
            TournamentStage = TournamentStage.RoundOf64, PlayOrderNumber = 46, HomePrevMatch = 11, AwayPrevMatch = 12
        },
        47 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Winner of match 13", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 47, HomePrevMatch = null, AwayPrevMatch = 13
        },
        48 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 20", AwayPlayer = "Winner of match 14", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 48, HomePrevMatch = null, AwayPrevMatch = 14
        },
        49 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Winner of match 15", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 49, HomePrevMatch = null, AwayPrevMatch = 15
        },
        50 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 28", AwayPlayer = "Winner of match 16", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 50, HomePrevMatch = null, AwayPrevMatch = 16
        },
        51 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Winner of match 17", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 51, HomePrevMatch = null, AwayPrevMatch = 17
        },
        52 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 21", AwayPlayer = "Winner of match 18", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 52, HomePrevMatch = null, AwayPrevMatch = 18
        },
        53 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 19", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 53, HomePrevMatch = null, AwayPrevMatch = 19
        },
        54 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 20", AwayPlayer = "Winner of match 21",
            TournamentStage = TournamentStage.RoundOf64, PlayOrderNumber = 54, HomePrevMatch = 20, AwayPrevMatch = 21
        },
        55 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Winner of match 22", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 55, HomePrevMatch = null, AwayPrevMatch = 22
        },
        56 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 18", AwayPlayer = "Winner of match 23", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 56, HomePrevMatch = null, AwayPrevMatch = 23
        },
        57 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Winner of match 24", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 57, HomePrevMatch = null, AwayPrevMatch = 24
        },
        58 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 26", AwayPlayer = "Winner of match 25", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 58, HomePrevMatch = null, AwayPrevMatch = 25
        },
        59 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Winner of match 26", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 59, HomePrevMatch = null, AwayPrevMatch = 26
        },
        60 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 23", AwayPlayer = "Winner of match 27", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 60, HomePrevMatch = null, AwayPrevMatch = 27
        },
        61 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 28", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 61, HomePrevMatch = null, AwayPrevMatch = 28
        },
        62 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 29", AwayPlayer = "Winner of match 30",
            TournamentStage = TournamentStage.RoundOf64, PlayOrderNumber = 62, HomePrevMatch = 29, AwayPrevMatch = 30
        },
        63 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Winner of match 31", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 63, HomePrevMatch = null, AwayPrevMatch = 31
        },
        64 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 19", AwayPlayer = "Winner of match 32", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 64, HomePrevMatch = null, AwayPrevMatch = 32
        },
        65 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Winner of match 33", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 65, HomePrevMatch = null, AwayPrevMatch = 33
        },
        66 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 27", AwayPlayer = "Winner of match 34", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 66, HomePrevMatch = null, AwayPrevMatch = 34
        },
        67 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Winner of match 35", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 67, HomePrevMatch = null, AwayPrevMatch = 35
        },
        68 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 22", AwayPlayer = "Winner of match 36", TournamentStage = TournamentStage.RoundOf64,
            PlayOrderNumber = 68, HomePrevMatch = null, AwayPrevMatch = 36
        },
        69 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 37", AwayPlayer = "Winner of match 38",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 69, HomePrevMatch = 37, AwayPrevMatch = 38
        },
        70 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 39", AwayPlayer = "Winner of match 40",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 70, HomePrevMatch = 39, AwayPrevMatch = 40
        },
        71 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 41", AwayPlayer = "Winner of match 42",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 71, HomePrevMatch = 41, AwayPrevMatch = 42
        },
        72 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 43", AwayPlayer = "Winner of match 44",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 72, HomePrevMatch = 43, AwayPrevMatch = 44
        },
        73 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 45", AwayPlayer = "Winner of match 46",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 73, HomePrevMatch = 45, AwayPrevMatch = 46
        },
        74 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 47", AwayPlayer = "Winner of match 48",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 74, HomePrevMatch = 47, AwayPrevMatch = 48
        },
        75 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 49", AwayPlayer = "Winner of match 50",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 75, HomePrevMatch = 49, AwayPrevMatch = 50
        },
        76 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 51", AwayPlayer = "Winner of match 52",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 76, HomePrevMatch = 51, AwayPrevMatch = 52
        },
        77 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 53", AwayPlayer = "Winner of match 54",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 77, HomePrevMatch = 53, AwayPrevMatch = 54
        },
        78 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 55", AwayPlayer = "Winner of match 56",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 78, HomePrevMatch = 55, AwayPrevMatch = 56
        },
        79 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 57", AwayPlayer = "Winner of match 58",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 79, HomePrevMatch = 57, AwayPrevMatch = 58
        },
        80 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 59", AwayPlayer = "Winner of match 60",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 80, HomePrevMatch = 59, AwayPrevMatch = 60
        },
        81 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 61", AwayPlayer = "Winner of match 62",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 81, HomePrevMatch = 61, AwayPrevMatch = 62
        },
        82 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 63", AwayPlayer = "Winner of match 64",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 82, HomePrevMatch = 63, AwayPrevMatch = 64
        },
        83 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 65", AwayPlayer = "Winner of match 66",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 83, HomePrevMatch = 65, AwayPrevMatch = 66
        },
        84 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 67", AwayPlayer = "Winner of match 68",
            TournamentStage = TournamentStage.RoundOf32, PlayOrderNumber = 84, HomePrevMatch = 67, AwayPrevMatch = 68
        },
        85 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 69", AwayPlayer = "Winner of match 70",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 85, HomePrevMatch = 69, AwayPrevMatch = 70
        },
        86 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 71", AwayPlayer = "Winner of match 72",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 86, HomePrevMatch = 71, AwayPrevMatch = 72
        },
        87 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 73", AwayPlayer = "Winner of match 74",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 87, HomePrevMatch = 73, AwayPrevMatch = 74
        },
        88 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 75", AwayPlayer = "Winner of match 76",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 88, HomePrevMatch = 75, AwayPrevMatch = 76
        },
        89 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 77", AwayPlayer = "Winner of match 78",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 89, HomePrevMatch = 77, AwayPrevMatch = 78
        },
        90 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 79", AwayPlayer = "Winner of match 80",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 90, HomePrevMatch = 79, AwayPrevMatch = 80
        },
        91 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 81", AwayPlayer = "Winner of match 82",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 91, HomePrevMatch = 81, AwayPrevMatch = 82
        },
        92 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 83", AwayPlayer = "Winner of match 84",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 92, HomePrevMatch = 83, AwayPrevMatch = 84
        },
        93 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 85", AwayPlayer = "Winner of match 86",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 93, HomePrevMatch = 85, AwayPrevMatch = 86
        },
        94 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 87", AwayPlayer = "Winner of match 88",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 94, HomePrevMatch = 87, AwayPrevMatch = 88
        },
        95 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 89", AwayPlayer = "Winner of match 90",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 95, HomePrevMatch = 89, AwayPrevMatch = 90
        },
        96 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 91", AwayPlayer = "Winner of match 92",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 96, HomePrevMatch = 91, AwayPrevMatch = 92
        },
        97 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 93", AwayPlayer = "Winner of match 94",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 97, HomePrevMatch = 93, AwayPrevMatch = 94
        },
        98 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 95", AwayPlayer = "Winner of match 96",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 98, HomePrevMatch = 95, AwayPrevMatch = 96
        },
        99 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 97", AwayPlayer = "Winner of match 98",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 99, HomePrevMatch = 97, AwayPrevMatch = 98
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor40Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 32", AwayPlayer = "Seed 33", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 25", AwayPlayer = "Seed 40", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 29", AwayPlayer = "Seed 36", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 28", AwayPlayer = "Seed 37", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 31", AwayPlayer = "Seed 34", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 26", AwayPlayer = "Seed 39", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 30", AwayPlayer = "Seed 35", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 27", AwayPlayer = "Seed 38", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 17, HomePrevMatch = null, AwayPrevMatch = 1
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Seed 17", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 18, HomePrevMatch = null, AwayPrevMatch = 2
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Seed 24", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 19, HomePrevMatch = null, AwayPrevMatch = 3
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Seed 20", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 20, HomePrevMatch = null, AwayPrevMatch = 4
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Seed 21", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 5", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 21, HomePrevMatch = null, AwayPrevMatch = 5
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Seed 18", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Winner of match 6", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 22, HomePrevMatch = null, AwayPrevMatch = 6
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Seed 23", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 7", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 23, HomePrevMatch = null, AwayPrevMatch = 7
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Seed 19", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Winner of match 8", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 24, HomePrevMatch = null, AwayPrevMatch = 8
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Seed 22", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = null
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 9", AwayPlayer = "Winner of match 10",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 25, HomePrevMatch = 9, AwayPrevMatch = 10
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 11", AwayPlayer = "Winner of match 12",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 26, HomePrevMatch = 11, AwayPrevMatch = 12
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 13", AwayPlayer = "Winner of match 14",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 27, HomePrevMatch = 13, AwayPrevMatch = 14
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 15", AwayPlayer = "Winner of match 16",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 28, HomePrevMatch = 15, AwayPrevMatch = 16
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 17", AwayPlayer = "Winner of match 18",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 29, HomePrevMatch = 17, AwayPrevMatch = 18
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 19", AwayPlayer = "Winner of match 20",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 30, HomePrevMatch = 19, AwayPrevMatch = 20
        },
        31 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 21", AwayPlayer = "Winner of match 22",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 31, HomePrevMatch = 21, AwayPrevMatch = 22
        },
        32 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 23", AwayPlayer = "Winner of match 24",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 32, HomePrevMatch = 23, AwayPrevMatch = 24
        },
        33 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 25", AwayPlayer = "Winner of match 26",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 33, HomePrevMatch = 25, AwayPrevMatch = 26
        },
        34 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 27", AwayPlayer = "Winner of match 28",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 34, HomePrevMatch = 27, AwayPrevMatch = 28
        },
        35 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 29", AwayPlayer = "Winner of match 30",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 35, HomePrevMatch = 29, AwayPrevMatch = 30
        },
        36 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 31", AwayPlayer = "Winner of match 32",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 36, HomePrevMatch = 31, AwayPrevMatch = 32
        },
        37 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 33", AwayPlayer = "Winner of match 34",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 37, HomePrevMatch = 33, AwayPrevMatch = 34
        },
        38 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 35", AwayPlayer = "Winner of match 36",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 38, HomePrevMatch = 35, AwayPrevMatch = 36
        },
        39 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 37", AwayPlayer = "Winner of match 38",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 39, HomePrevMatch = 37, AwayPrevMatch = 38
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };

    private static MatchGeneratorOutput GetExpectedMatchFor36Participants(int matchNumber) => matchNumber switch
    {
        1 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 32", AwayPlayer = "Seed 33", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 1, HomePrevMatch = null, AwayPrevMatch = null
        },
        2 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 29", AwayPlayer = "Seed 36", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 2, HomePrevMatch = null, AwayPrevMatch = null
        },
        3 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 31", AwayPlayer = "Seed 34", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 3, HomePrevMatch = null, AwayPrevMatch = null
        },
        4 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 30", AwayPlayer = "Seed 35", TournamentStage = TournamentStage.Qualification,
            PlayOrderNumber = 4, HomePrevMatch = null, AwayPrevMatch = null
        },
        5 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 1", AwayPlayer = "Winner of match 1", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 17, HomePrevMatch = null, AwayPrevMatch = 1
        },
        6 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 16", AwayPlayer = "Seed 17", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 5, HomePrevMatch = null, AwayPrevMatch = null
        },
        7 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 8", AwayPlayer = "Seed 25", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 6, HomePrevMatch = null, AwayPrevMatch = null
        },
        8 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 9", AwayPlayer = "Seed 24", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 7, HomePrevMatch = null, AwayPrevMatch = null
        },
        9 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 4", AwayPlayer = "Winner of match 2", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 18, HomePrevMatch = null, AwayPrevMatch = 2
        },
        10 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 13", AwayPlayer = "Seed 20", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 8, HomePrevMatch = null, AwayPrevMatch = null
        },
        11 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 5", AwayPlayer = "Seed 28", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 9, HomePrevMatch = null, AwayPrevMatch = null
        },
        12 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 12", AwayPlayer = "Seed 21", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 10, HomePrevMatch = null, AwayPrevMatch = null
        },
        13 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 2", AwayPlayer = "Winner of match 3", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 19, HomePrevMatch = null, AwayPrevMatch = 3
        },
        14 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 15", AwayPlayer = "Seed 18", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 11, HomePrevMatch = null, AwayPrevMatch = null
        },
        15 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 7", AwayPlayer = "Seed 26", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 12, HomePrevMatch = null, AwayPrevMatch = null
        },
        16 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 10", AwayPlayer = "Seed 23", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 13, HomePrevMatch = null, AwayPrevMatch = null
        },
        17 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 3", AwayPlayer = "Winner of match 4", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 20, HomePrevMatch = null, AwayPrevMatch = 4
        },
        18 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 14", AwayPlayer = "Seed 19", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 14, HomePrevMatch = null, AwayPrevMatch = null
        },
        19 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 6", AwayPlayer = "Seed 27", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 15, HomePrevMatch = null, AwayPrevMatch = null
        },
        20 => new MatchGeneratorOutput
        {
            HomePlayer = "Seed 11", AwayPlayer = "Seed 22", TournamentStage = TournamentStage.RoundOf32,
            PlayOrderNumber = 16, HomePrevMatch = null, AwayPrevMatch = null
        },
        21 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 5", AwayPlayer = "Winner of match 6",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 21, HomePrevMatch = 5, AwayPrevMatch = 6
        },
        22 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 7", AwayPlayer = "Winner of match 8",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 22, HomePrevMatch = 7, AwayPrevMatch = 8
        },
        23 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 9", AwayPlayer = "Winner of match 10",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 23, HomePrevMatch = 9, AwayPrevMatch = 10
        },
        24 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 11", AwayPlayer = "Winner of match 12",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 24, HomePrevMatch = 11, AwayPrevMatch = 12
        },
        25 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 13", AwayPlayer = "Winner of match 14",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 25, HomePrevMatch = 13, AwayPrevMatch = 14
        },
        26 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 15", AwayPlayer = "Winner of match 16",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 26, HomePrevMatch = 15, AwayPrevMatch = 16
        },
        27 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 17", AwayPlayer = "Winner of match 18",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 27, HomePrevMatch = 17, AwayPrevMatch = 18
        },
        28 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 19", AwayPlayer = "Winner of match 20",
            TournamentStage = TournamentStage.RoundOf16, PlayOrderNumber = 28, HomePrevMatch = 19, AwayPrevMatch = 20
        },
        29 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 21", AwayPlayer = "Winner of match 22",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 29, HomePrevMatch = 21, AwayPrevMatch = 22
        },
        30 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 23", AwayPlayer = "Winner of match 24",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 30, HomePrevMatch = 23, AwayPrevMatch = 24
        },
        31 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 25", AwayPlayer = "Winner of match 26",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 31, HomePrevMatch = 25, AwayPrevMatch = 26
        },
        32 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 27", AwayPlayer = "Winner of match 28",
            TournamentStage = TournamentStage.QuarterFinal, PlayOrderNumber = 32, HomePrevMatch = 27, AwayPrevMatch = 28
        },
        33 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 29", AwayPlayer = "Winner of match 30",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 33, HomePrevMatch = 29, AwayPrevMatch = 30
        },
        34 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 31", AwayPlayer = "Winner of match 32",
            TournamentStage = TournamentStage.SemiFinal, PlayOrderNumber = 34, HomePrevMatch = 31, AwayPrevMatch = 32
        },
        35 => new MatchGeneratorOutput
        {
            HomePlayer = "Winner of match 33", AwayPlayer = "Winner of match 34",
            TournamentStage = TournamentStage.Final, PlayOrderNumber = 35, HomePrevMatch = 33, AwayPrevMatch = 34
        },
        _ => new MatchGeneratorOutput {TournamentStage = TournamentStage.Unknown} // Default case
    };
}