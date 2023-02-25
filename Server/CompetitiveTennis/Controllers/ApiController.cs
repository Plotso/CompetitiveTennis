namespace CompetitiveTennis.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
    protected const int UnexpectedErrorCode = -666;
    public const string PathSeparator = "/";
    public const string Id = "{id}";
}
