using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Exceptions;

namespace ProductCatalogue.Api.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    AppDbContext context,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly AppDbContext _context = context;
    private readonly ILogger<AuthService> _logger = logger;

    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);

    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        _logger.LogInformation("[Auth] Login attempt for {Email}", dto.Email);

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedException("Invalid email or password");

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshToken = await IssueRefreshTokenAsync(user.Id);

        _logger.LogInformation("[Auth] User {UserId} logged in successfully", user.Id);
        return BuildResult(user, accessToken, refreshToken);
    }

    public async Task<AuthResult> RefreshAsync(string? refreshToken)
    {
        _logger.LogInformation("[Auth] Refresh token requested");

        if (refreshToken is null)
            throw new UnauthorizedException("Refresh token missing");

        var stored = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (stored is null || !stored.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token");

        stored.RevokedAt = DateTimeOffset.UtcNow;

        var user = stored.User!;
        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _tokenService.GenerateAccessToken(user, roles);
        var newRefreshToken = await IssueRefreshTokenAsync(user.Id);

        _logger.LogInformation("[Auth] Refresh token rotated for user {UserId}", user.Id);
        return BuildResult(user, newAccessToken, newRefreshToken);
    }

    public async Task LogoutAsync(string? refreshToken)
    {
        if (refreshToken is null) return;

        var stored = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (stored is not null)
        {
            stored.RevokedAt = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogInformation("[Auth] User {UserId} logged out; refresh token revoked", stored.UserId);
        }
    }

    private async Task<string> IssueRefreshTokenAsync(string userId)
    {
        var token = _tokenService.GenerateRefreshToken();
        _context.RefreshTokens.Add(new RefreshToken
        {
            Token = token,
            UserId = userId,
            ExpiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifetime)
        });
        await _context.SaveChangesAsync();
        return token;
    }

    private static AuthResult BuildResult(AppUser user, string accessToken, string refreshToken) => new()
    {
        Response = new AuthResponseDto
        {
            AccessToken = accessToken,
            Email = user.Email!,
            FullName = user.FullName
        },
        RefreshToken = refreshToken
    };
}
