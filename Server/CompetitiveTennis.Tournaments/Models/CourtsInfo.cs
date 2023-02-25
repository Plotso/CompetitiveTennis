namespace CompetitiveTennis.Tournaments.Models;

using CompetitiveTennis.Tournaments.Data.Models.Enums;
using Enums;

public class CourtsInfo
{
    public Surface Surface { get; set; }
    public Dictionary<CourtType, int> AvailableCourtsByType { get; set; }
}