using MediatR;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Domain;
using MobilitywaysAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilitywaysAPI.Application.Users.Queries.Login;

public record LoginUserQuery : IRequest<string>
{
    public UserLoginDto UserLogin { get; set; }
}

public class Handler : IRequestHandler<LoginUserQuery, string>
{
    private readonly IRepository<User> _userRepository;

    public Handler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        // TODO: Check user exists and verify password, then create JWT
        throw new NotImplementedException();
    }
}
