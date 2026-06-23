using ProductCatalogue.Api.DTOs;

namespace ProductCatalogue.Api.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginDto dto);
    Task<AuthResult> RefreshAsync(string? refreshToken);
    Task LogoutAsync(string? refreshToken);
}

public class AuthResult
{
    public required AuthResponseDto Response { get; init; }
    public required string RefreshToken { get; init; }
}
