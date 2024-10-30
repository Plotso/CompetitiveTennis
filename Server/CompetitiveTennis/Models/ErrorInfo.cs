namespace CompetitiveTennis.Models;
using static Constants;

public record ErrorInfo(int ErrorCode, string Message)
{
    public static ErrorInfo UnexpectedError(string message) => new(ErrorCodes.UnexpectedErrorCode, message);
    public static ErrorInfo InvalidInputError(string message) => new(ErrorCodes.InvalidInputErrorCode, message);
    public static ErrorInfo ProvidedDataNotFoundError(string message) => new(ErrorCodes.ProvidedDataNotFound, message);
    
    public override string ToString() => $"ErrorCode: {ErrorCode}, ErrorMessage: {Message}";
}