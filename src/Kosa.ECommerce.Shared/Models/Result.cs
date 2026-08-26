namespace Kosa.ECommerce.Shared.Models;

public sealed class Result<T>
{
    public bool Success { get; init; } // t / f success

    public string Message { get; init; } = string.Empty; // 

    public T? Data { get; init; } // data success

    public static Result<T> Ok(T data, string message = "Request completed successfully.") // Successful response
    {
        return new Result<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static Result<T> Fail(string message) // Failed response
    {
        return new Result<T>
        {
            Success = false,
            Message = message
        };
    }
}
