namespace Hemeroteca.API.Services.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(string username, string password);
    Task<bool> RegistroAsync(string username, string password);
}
