using MediatR;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.Domain;
using MobilitywaysAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilitywaysAPI.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery() : IRequest<IEnumerable<UserViewDto>>;

public class Handler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserViewDto>>
{
    private readonly IRepository<User> _userRepository;

    public Handler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserViewDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return _userRepository.GetAll().Select(_ => new UserViewDto
        {
            Name = _.Name,
            Email = _.Email,
        });
    }
}
