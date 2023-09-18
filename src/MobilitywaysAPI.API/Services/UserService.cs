using MediatR;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Application.Users.Commands.CreateUser;

namespace MobilitywaysAPI.API.Services;

public interface IUserService
{
    Task CreateUser(UserDto user);
}

public class UserService : IUserService
{
    private readonly IMediator _mediator;

    public UserService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task CreateUser(UserDto user)
    {
        await _mediator.Send(new CreateUserCommand { User = user });
    }
}
