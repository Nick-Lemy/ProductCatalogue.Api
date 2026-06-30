namespace ProductCatalogue.Api.DTOs;
public enum ErrorType
{
    NotFound = 0,
    Conflict = 1,
    Validation = 2,
    Unauthorized = 3
}

public record Error(ErrorType Type, string Message);

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);

    public static implicit operator Result(Error error) => Failure(error);

    public static Result NotFound(string message) => Failure(new Error(ErrorType.NotFound, message));
    public static Result Conflict(string message) => Failure(new Error(ErrorType.Conflict, message));
    public static Result Validation(string message) => Failure(new Error(ErrorType.Validation, message));
    public static Result Unauthorized(string message) => Failure(new Error(ErrorType.Unauthorized, message));
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public Error? Error { get; }
    public T? Value { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Result result) => new(result.Error!);
}
