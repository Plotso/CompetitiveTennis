namespace CompetitiveTennis.Tournaments.Contracts;

using Data.Models.Enums;
using Enums;

public class CourtsInfo
{
    public Surface Surface { get; set; }
    public Dictionary<CourtType, int> AvailableCourtsByType { get; set; }
}