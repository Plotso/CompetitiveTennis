namespace CompetitiveTennis.Tournaments.Gateway.Controllers;

using Services.Tournaments;

public class TournamentsController : BaseGatewayController
{
    private readonly ITournamentsService _tournaments;

    public TournamentsController(ITournamentsService tournaments, ILogger logger) 
        : base(logger) 
        => _tournaments = tournaments;
    
    //ToDo: Expose tournaments endpoints for tournament controller 
}