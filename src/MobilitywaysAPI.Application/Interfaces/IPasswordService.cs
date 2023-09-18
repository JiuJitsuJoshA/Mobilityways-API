namespace MobilitywaysAPI.Application.Interfaces;
public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string requestPassword, string password);
}