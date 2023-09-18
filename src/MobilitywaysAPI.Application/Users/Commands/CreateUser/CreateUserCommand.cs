using MediatR;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Application.Interfaces;
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
    private readonly IPasswordService _passwordService;

    public Handler(IRepository<User> userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = _userRepository.GetAll().Any(_ => _.Email.Equals(request.User.Email, StringComparison.InvariantCultureIgnoreCase));

        if (!userExists)
        {
            // Return user already exists
        }

        var passwordHash = _passwordService.HashPassword(request.User.Password);

        var userToAdd = new User(request.User.Name, request.User.Email, passwordHash);

        await _userRepository.AddAsync(userToAdd);
        await _userRepository.SaveChangesAsync();
    }
}
