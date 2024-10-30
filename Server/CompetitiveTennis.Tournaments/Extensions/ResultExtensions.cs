namespace CompetitiveTennis.Tournaments.Extensions;

using CompetitiveTennis.Models;
using Microsoft.AspNetCore.Mvc;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result) 
        => result.IsSuccess ? 
            new OkObjectResult(result) :
            new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
    public static ActionResult ToActionResult<T>(this Result<T> result) 
        => result.IsSuccess ? 
            new OkObjectResult(result) :
            new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
}