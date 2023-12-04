namespace CompetitiveTennis.Tournaments.Services.BL;

using CompetitiveTennis.Data.Models.Enums;
using Models.TournamentDrawGenerator;

public static class MatchScheduler
{
    public static void ScheduleMatches(List<MatchGeneratorOutput> matches, DateTime tournamentStartDate, DateTime tournamentEndDate, int availableCourts, TimeSpan startTime, TimeSpan endTime)
    {
        var tournamentPeriod = tournamentEndDate - tournamentStartDate;
        var tournamentDays = (int)Math.Ceiling(tournamentPeriod.TotalDays);
        var totalRounds = matches.GroupBy(m => m.TournamentStage).Count();
        var matchDuration = TimeSpan.FromHours(1);
        var dayOneStartTime = new TimeSpan(tournamentStartDate.Hour, tournamentStartDate.Minute, tournamentStartDate.Second);
        var finalEndTime =  new TimeSpan(tournamentEndDate.Hour, tournamentEndDate.Minute, tournamentEndDate.Second);

        // Sort matches by stage and then by ID
        var sortedMatches = matches.OrderBy(m => m.TournamentStage).ThenBy(m => m.Id).ToList();

        var matchesByStage = sortedMatches.GroupBy(m => m.TournamentStage).ToList();

        if (totalRounds == tournamentDays)
        {
            // Each round can be played on a different day
            for (var i = 0; i < totalRounds; i++)
            {
                var currentRoundMatches = matchesByStage[i].ToList();
                var dayInput = tournamentStartDate.AddDays(i);
                var day = new DateTime(dayInput.Year, dayInput.Month, dayInput.Day);
                var roundStartTime = i == 0 ? dayOneStartTime : startTime;
                var roundEndTime = i == totalRounds - 1 ? finalEndTime : endTime;
                ScheduleMatchesForDay(currentRoundMatches, day, availableCourts, matchDuration, roundStartTime, roundEndTime);
            }
        }
        else if (totalRounds < tournamentDays)
        {
            var hoursPerDay = Math.Floor((endTime - startTime).TotalHours);
            var maxMatchesPerDay = hoursPerDay * availableCourts;
            var firstRoundMatches = matchesByStage.First().Count();
            var remainingStartIndex = 0;

            if (firstRoundMatches > maxMatchesPerDay) // split matches across multiple days only if they can't fit in a single day (trying to avoid angry players + have some fairplay)
            {
                var additionalDays = tournamentDays - totalRounds;
                for (int i = 0; i < additionalDays; i+=2)
                {
                    var dayInput = tournamentStartDate.AddDays(i);
                    var day = new DateTime(dayInput.Year, dayInput.Month, dayInput.Day);
                    var roundStartTime = i == 0 ? dayOneStartTime : startTime;
                    var roundEndTime = i == totalRounds - 1 ? finalEndTime : endTime;
                
                    var currentRoundMatches = matchesByStage[i].ToList();
                    var firstDayMatchesCount = currentRoundMatches.Count() / 2;
                    var firstDayMatches = currentRoundMatches.Take(firstDayMatchesCount).ToList();
                    ScheduleMatchesForDay(firstDayMatches, day, availableCourts, matchDuration, roundStartTime, roundEndTime);
                    var secondDayMatches = currentRoundMatches.Skip(firstDayMatchesCount).ToList();
                    ScheduleMatchesForDay(secondDayMatches, day.AddDays(1), availableCourts, matchDuration, roundStartTime, roundEndTime);
                }
                remainingStartIndex = additionalDays;
            }
            // First rounds can be played across different days
            for (var i = remainingStartIndex; i < totalRounds; i++)
            {
                var currentRoundMatches = matchesByStage[i].ToList();
                var dayInput = tournamentStartDate.AddDays(i);
                var day = new DateTime(dayInput.Year, dayInput.Month, dayInput.Day);
                if (currentRoundMatches.Any(m => m.TournamentStage == TournamentStage.Final))
                    day = new DateTime(tournamentEndDate.Year, tournamentEndDate.Month, tournamentEndDate.Day);
                var roundStartTime = i == 0 ? dayOneStartTime : startTime;
                var roundEndTime = i == totalRounds - 1 ? finalEndTime : endTime;
                ScheduleMatchesForDay(currentRoundMatches, day, availableCourts, matchDuration, roundStartTime, roundEndTime);
            }
        }
        else
        {
            // Distribute matches over the available days
            var roundsPerDay = totalRounds / tournamentDays;
            var remainingRounds = totalRounds % tournamentDays;
            var generatedRounds = 0;
            var processedMatchIds = new List<int>();

            for (var i = 0; i < tournamentDays; i++)
            {
                var roundsForTheDay = roundsPerDay;
                if (remainingRounds > 0) // In case of pending rounds, move them to the first day
                {
                    roundsForTheDay += remainingRounds;
                    remainingRounds = 0;
                };
                var matchesForDay = matchesByStage
                    .Skip(generatedRounds)
                    .Take(roundsForTheDay)
                    .SelectMany(m => m)
                    .Where(m => !processedMatchIds.Contains(m.Id))
                    .ToList();

                var dayInput = tournamentStartDate.AddDays(i);
                var day = new DateTime(dayInput.Year, dayInput.Month, dayInput.Day);
                var roundStartTime = i == 0 ? dayOneStartTime : startTime;
                var roundEndTime = i == tournamentDays - 1 ? finalEndTime : endTime;
                ScheduleMatchesForDay(matchesForDay, day, availableCourts, matchDuration, roundStartTime, roundEndTime);
                generatedRounds += roundsPerDay;
                processedMatchIds.AddRange(matchesForDay.Select(m => m.Id));
            }
        }
    }
    
    private static void ScheduleMatchesForDay(List<MatchGeneratorOutput> matches, DateTime day, int availableCourts, TimeSpan matchDuration, TimeSpan startTime, TimeSpan endTime)
    {
        var matchesScheduled = new List<MatchGeneratorOutput>();
        foreach (var match in matches)
        {
            if (match.TournamentStage == TournamentStage.Final)
            {
                match.StartTime = day.Add(endTime);
                break;
            }
            if (availableCourts > 1)
            {
                var matchEndTime = day.Add(startTime).Add(matchDuration);

                while (true)
                {
                    var overlappingMatches = matchesScheduled.Where(m =>
                        m.StartTime >= day.Add(startTime) &&
                        m.StartTime < matchEndTime);

                    if (!overlappingMatches.Any() || overlappingMatches.Count() < availableCourts)
                    {
                        match.StartTime = day.Add(startTime);
                        matchesScheduled.Add(match);
                        break;
                    }

                    startTime = startTime.Add(TimeSpan.FromHours(1)); // Move to the next hour
                }
            }
            else
            {
                // If only one court is available, schedule matches consecutively
                match.StartTime = day.Add(startTime);
                startTime = startTime.Add(matchDuration);
                matchesScheduled.Add(match);
            }
        }
    }
}