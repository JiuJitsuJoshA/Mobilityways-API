using MobilitywayAPI.Shared;
using MobilitywaysAPI.API.Services;
using MobilitywaysAPI.Application.Result;

namespace MobilitywaysAPI.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/api/user/CreateUser", async (UserDto user, IUserService userService) =>
        {
            var result = await userService.CreateUserAsync(user);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Message);
            }

            return Results.Created("/api/user/CreateUser", result.Message);
        });

        app.MapPost("api/user/GetJWTToken", async (UserLoginDto userLogin, IUserService userService) =>
        {
            var result = await userService.GetJwtTokenAsync(userLogin);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Message);
            }

            return Results.Ok(result.Value);
        });

        app.MapGet("api/user/ListUsers", async (IUserService userService) =>
        {
            var result = await userService.GetAllUsersAsync();

            if (!result.IsSuccess)
            {
                if (result.Type == ResultType.NotFound)
                {
                    return Results.NotFound(result.Message);
                }
            }

            return Results.Ok(result.Value);
        })
        .RequireAuthorization();
    }
}
