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

public record GetAllUsersQuery() : IRequest<Result.Result<IEnumerable<UserViewDto>>>;

public class Handler : IRequestHandler<GetAllUsersQuery, Result.Result<IEnumerable<UserViewDto>>>
{
    private readonly IRepository<User> _userRepository;

    public Handler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result.Result<IEnumerable<UserViewDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = _userRepository.GetAll().Select(_ => new UserViewDto
            {
                Name = _.Name,
                Email = _.Email,
            }).AsEnumerable();

            return Result.Result.Success(Result.ResultType.Ok, users, string.Empty);
        }
        catch (Exception ex) 
        {
            // Log exception here

            return Result.Result.Exception<IEnumerable<UserViewDto>>();
        }
    }
}
