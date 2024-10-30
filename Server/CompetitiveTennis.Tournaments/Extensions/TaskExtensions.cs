namespace CompetitiveTennis.Tournaments.Extensions;

using CompetitiveTennis.Models;

public static class TaskExtensions
{
    public static async Task<Result> ToResultableTask(this Task task, string errorMessage = null)
    {
        try
        {
            await task;
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Result.Failure(string.IsNullOrWhiteSpace(errorMessage) ? ex.Message : errorMessage);
        }
    }
}