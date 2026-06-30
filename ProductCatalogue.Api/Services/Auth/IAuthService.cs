using ProductCatalogue.Api.DTOs;

namespace ProductCatalogue.Api.Services;

public interface IAuthService
{
    Task<Result<AuthResult>> LoginAsync(LoginDto dto);
    Task<Result<AuthResult>> RefreshAsync(string? refreshToken);
    Task<Result> LogoutAsync(string? refreshToken);
}

public class AuthResult
{
    public required AuthResponseDto Response { get; init; }
    public required string RefreshToken { get; init; }
}
