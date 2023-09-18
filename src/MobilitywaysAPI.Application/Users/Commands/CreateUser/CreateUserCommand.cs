using MediatR;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Domain;
using MobilitywaysAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilitywaysAPI.Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest
{
    public UserDto User { get; set; }
}

public class Handler : IRequestHandler<CreateUserCommand>
{
    private readonly IRepository<User> _userRepository;

    public Handler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // TODO: Check user does not already exists, hash password and then save to DB
    }
}
