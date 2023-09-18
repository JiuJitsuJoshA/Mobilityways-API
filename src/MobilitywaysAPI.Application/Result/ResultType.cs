namespace MobilitywaysAPI.Application.Result;

public enum ResultType
{
    Ok = 0,
    Created = 1,
    NotFound = 2,
    FailedValidation = 3,
    AlreadyExists = 4,
    Exception = 5
}
