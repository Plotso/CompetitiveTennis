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
    
    protected const int UnexpectedErrorCode = -666;
    protected const int InvalidInputErrorCode = -777;
    protected const int ProvidedDataNotFound = -888;
    protected ILogger<ApiController> Logger { get; }

    protected async Task<ActionResult> SafeHandle(
        Func<Task<ActionResult>> action,
        string msgOnError = "Unexpected error occurred",
        string msgOnNotFound = "Entry could not be found")
        => await SafeHandle(action,
            failure: StatusCode((int) HttpStatusCode.InternalServerError, Result.Failure($"ErrorCode: {UnexpectedErrorCode}")),
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
        catch (MissingEntryException exception)
        {
            Logger.LogError(exception, msgOnNotFound);
            return NotFound(Result.Failure($"ErrorCode: {ProvidedDataNotFound}"));
        }
        catch (InvalidInputDataException ex)
        {
            Logger.LogError(ex, ex.Message);
            return BadRequest(Result.Failure($"ErrorCode: {InvalidInputErrorCode}, ErrorMessage: {ex.Message}"));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, msgOnError);
            return failure;
        }
    }
}
