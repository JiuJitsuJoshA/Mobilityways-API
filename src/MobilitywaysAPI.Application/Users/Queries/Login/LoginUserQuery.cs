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

namespace MobilitywaysAPI.Application.Users.Queries.Login;

public record LoginUserQuery : IRequest<Result.Result<string>>
{
    public UserLoginDto UserLogin { get; set; }
}

public class Handler : IRequestHandler<LoginUserQuery, Result.Result<string>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public Handler(IRepository<User> userRepository, IPasswordService passwordService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<Result.Result<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = _userRepository.GetAll()
                                        .Where(_ => _.Email.Equals(request.UserLogin.Email, StringComparison.InvariantCultureIgnoreCase))
                                        .FirstOrDefault();
            if (user == null)
            {
                return Result.Result.Failure<string>(Result.ResultType.NotFound, "User not found or password incorrect");
            }

            if (!_passwordService.VerifyPassword(request.UserLogin.Password, user.PasswordHash))
            {
                return Result.Result.Failure<string>(Result.ResultType.NotFound, "User not found or password incorrect");
            }

            // Generate Token
            return Result.Result.Success(Result.ResultType.Ok, _jwtService.GenerateToken(request.UserLogin), string.Empty);
        }
        catch (Exception ex)
        {
            // Log exception

            return Result.Result.Exception<string>();
        }
    }
}
