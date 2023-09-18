using MediatR;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Application.Result;
using MobilitywaysAPI.Application.Users.Commands.CreateUser;
using MobilitywaysAPI.Application.Users.Queries.GetAllUsers;
using MobilitywaysAPI.Application.Users.Queries.Login;

namespace MobilitywaysAPI.API.Services;

public interface IUserService
{
    Task<Result> CreateUser(UserDto user);
    Task<string> GetJwtToken(UserLoginDto userLogin);
    Task<IEnumerable<UserViewDto>> GetAllUsers();
}

public class UserService : IUserService
{
    private readonly IMediator _mediator;

    public UserService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result> CreateUser(UserDto user)
    {
        return await _mediator.Send(new CreateUserCommand { User = user });
    }

    public async Task<string> GetJwtToken(UserLoginDto userLogin)
    {
        return await _mediator.Send(new LoginUserQuery { UserLogin = userLogin });
    }

    public async Task<IEnumerable<UserViewDto>> GetAllUsers()
    {
        return await _mediator.Send(new GetAllUsersQuery());
    }
}
