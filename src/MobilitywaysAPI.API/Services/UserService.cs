using MediatR;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Application.Result;
using MobilitywaysAPI.Application.Users.Commands.CreateUser;
using MobilitywaysAPI.Application.Users.Queries.GetAllUsers;
using MobilitywaysAPI.Application.Users.Queries.Login;
using MobilitywaysAPI.Application.Users.Queries.ValidateUser;

namespace MobilitywaysAPI.API.Services;

public interface IUserService
{
    Task<Result> CreateUserAsync(UserDto user);
    Task<Result<string>> GetJwtTokenAsync(UserLoginDto userLogin);
    Task<Result<IEnumerable<UserViewDto>>> GetAllUsersAsync();
    Task<Result> ValidateUser(string email);
}

public class UserService : IUserService
{
    private readonly IMediator _mediator;

    public UserService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result> CreateUserAsync(UserDto user)
    {
        return await _mediator.Send(new CreateUserCommand { User = user });
    }

    public async Task<Result<string>> GetJwtTokenAsync(UserLoginDto userLogin)
    {
        return await _mediator.Send(new LoginUserQuery { UserLogin = userLogin });
    }

    public async Task<Result<IEnumerable<UserViewDto>>> GetAllUsersAsync()
    {
        return await _mediator.Send(new GetAllUsersQuery());
    }

    public async Task<Result> ValidateUser(string email)
    {
        return await _mediator.Send(new ValidateUserQuery(email));
    }
}
