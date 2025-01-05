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
    protected ApiController()
    { }
    protected ApiController(ILogger<ApiController> logger)
    {
        Logger = logger;
    }
    public const string PathSeparator = "/";
    public const string Id = "{id}";
    
    protected ILogger<ApiController> Logger { get; }

    protected async Task<ActionResult> SafeHandle(
        Func<Task<ActionResult>> action,
        string msgOnError = "Unexpected error occurred",
        string msgOnNotFound = "Entry could not be found")
        => await SafeHandle(action,
            failure: StatusCode((int) HttpStatusCode.InternalServerError, Result.Failure(ErrorInfo.UnexpectedError(msgOnError))),
            msgOnError, msgOnNotFound);
    
    protected async Task<ActionResult> SafeHandle(
        Func<Task<ActionResult>> action,
        ActionResult failure,
        string msgOnError = "Unexpected error occurred",
        string msgOnNotFound = "Entry could not be found")
    {
        try
        {
            return await action();
        }
        catch (MissingEntryException ex)
        {
            Logger.LogError(ex, ex.Message);
            return NotFound(Result.Failure(ErrorInfo.ProvidedDataNotFoundError(msgOnNotFound)));
        }
        catch (InvalidInputDataException ex)
        {
            Logger.LogError(ex, ex.Message);
            return NotFound(Result.Failure(ErrorInfo.InvalidInputError(msgOnError)));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            return failure;
        }
    }
}
