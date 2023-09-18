using MobilitywaysAPI.Application.Interfaces;

namespace MobilitywaysAPI.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string requestPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(requestPassword, password);
    }
}
