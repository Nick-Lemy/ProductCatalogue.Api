using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using UnauthorizedException = ProductCatalogue.Api.Exceptions.UnauthorizedAccessException;

namespace ProductCatalogue.Api.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    AppDbContext context) : IAuthService
{
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);

    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedException("Invalid email or password");

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = await IssueRefreshTokenAsync(user.Id);

        return BuildResult(user, accessToken, refreshToken);
    }

    public async Task<AuthResult> RefreshAsync(string? refreshToken)
    {
        if (refreshToken is null)
            throw new UnauthorizedException("Refresh token missing");

        var stored = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (stored is null || !stored.IsActive)
            throw new UnauthorizedException("Invalid or expired refresh token");

        // rotation — revoke old, issue new
        stored.RevokedAt = DateTimeOffset.UtcNow;

        var user = stored.User!;
        var roles = await userManager.GetRolesAsync(user);
        var newAccessToken = tokenService.GenerateAccessToken(user, roles);
        var newRefreshToken = await IssueRefreshTokenAsync(user.Id);

        return BuildResult(user, newAccessToken, newRefreshToken);
    }

    public async Task LogoutAsync(string? refreshToken)
    {
        if (refreshToken is null) return;

        var stored = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (stored is not null)
        {
            stored.RevokedAt = DateTimeOffset.UtcNow;
            await context.SaveChangesAsync();
        }
    }

    private async Task<string> IssueRefreshTokenAsync(string userId)
    {
        var token = tokenService.GenerateRefreshToken();
        context.RefreshTokens.Add(new RefreshToken
        {
            Token = token,
            UserId = userId,
            ExpiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifetime)
        });
        await context.SaveChangesAsync();
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
