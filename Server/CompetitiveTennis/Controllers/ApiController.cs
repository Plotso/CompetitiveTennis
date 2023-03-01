namespace CompetitiveTennis.Controllers;

using System.Net;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

[ApiController]
[Route("[controller]")]
public abstract class ApiController : ControllerBase
{
    protected ApiController(ILogger<ApiController> logger)
    {
        Logger = logger;
    }
    public const string PathSeparator = "/";
    public const string Id = "{id}";
    
    protected const int UnexpectedErrorCode = -666;
    protected ILogger<ApiController> Logger { get; }

    protected async Task<ActionResult> SafeHandle(
        Func<Task<ActionResult>> action,
        string msgOnError = "Unexpected error occured",
        string msgOnNotFound = "Entry could not be found")
        => await SafeHandle(action,
            failure: StatusCode((int) HttpStatusCode.InternalServerError, Result.Failure($"ErrorCode: {UnexpectedErrorCode}")),
            msgOnError, msgOnNotFound);
    
    protected async Task<ActionResult> SafeHandle(
        Func<Task<ActionResult>> action,
        ActionResult failure,
        string msgOnError = "Unexpected error occured",
        string msgOnNotFound = "Entry could not be found")
    {
        try
        {
            return await action();
        }
        catch (MissingEntryException exception)
        {
            Logger.LogError(exception, msgOnNotFound);
            return NotFound();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, msgOnError);
            return failure;
        }
    }
}
