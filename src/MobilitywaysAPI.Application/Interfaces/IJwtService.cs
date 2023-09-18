using MobilitywayAPI.Shared;

namespace MobilitywaysAPI.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserLoginDto userLogin);
}