namespace BoardGames.Domain.Validation;

public sealed class OperationResult
{
    public bool Success { get; }
    public string Message { get; }

    private OperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static OperationResult Ok(string message = "OK")
    {
        return new OperationResult(true, message);
    }

    public static OperationResult Fail(string message)
    {
        return new OperationResult(false, message);
    }
}