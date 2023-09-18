namespace MobilitywaysAPI.Application.Result;

public class Result
{
    public ResultType Type { get; }
    public bool IsSuccess { get; }
    public string Message { get; }

    public Result(ResultType type, bool isSuccess, string message)
    {
        Type = type;
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success(ResultType resultType, string message)
    {
        return new Result(resultType, true, message);
    }

    public static Result<T> Success<T>(ResultType resultType, T value, string message)
    {
        return new Result<T>(value, resultType, true, message);
    }

    public static Result Failure(ResultType resultType, string message)
    {
        return new Result(resultType, false, message);
    }

    public static Result<T> Failure<T>(ResultType resultType, string message)
    {
        return new Result<T>(default, resultType, false, message);
    }

    public static Result Exception()
    {
        return new Result(ResultType.Exception, false, "Something went wrong");
    }

    public static Result<T> Exception<T>()
    {
        return new Result<T>(default, ResultType.Exception, false, "Something went wrong");
    }
}

public class Result<T> : Result
{
    public T Value { get; }

    public Result(T value, ResultType type, bool isSuccess, string message) : base(type, isSuccess, message)
    {
        Value = value;
    }
}
