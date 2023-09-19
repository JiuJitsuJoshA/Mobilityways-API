using MediatR;
using MobilitywaysAPI.Application.Result;
using MobilitywaysAPI.Domain.Entities;
using MobilitywaysAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilitywaysAPI.Application.Users.Queries.ValidateUser;

public record ValidateUserQuery(string Email) : IRequest<Result.Result>;

public class Handler : IRequestHandler<ValidateUserQuery, Result.Result>
{
    private readonly IRepository<User> _userRepository;

    public Handler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result.Result> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = _userRepository.GetAll()
                                  .Where(_ => _.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase))
                                  .FirstOrDefault();

            if (user == null)
            {
                return Result.Result.Failure(ResultType.FailedValidation, string.Empty);
            }

            return Result.Result.Success(ResultType.Ok, string.Empty);

        }
        catch (Exception ex)
        {
            // Log exception

            return Result.Result.Exception();
        }                                   
    }
}
