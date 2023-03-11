namespace CompetitiveTennis.Tournaments.Gateway.Controllers;

using Services.Tournaments;

public class AvenuesController : BaseGatewayController
{
    private readonly IAvenuesService _avenues;

    public AvenuesController(IAvenuesService avenues, ILogger logger) 
        : base(logger) 
        => _avenues = avenues;
    
    //ToDo: Expose gateway endpoints for avenue controller endpoints
}