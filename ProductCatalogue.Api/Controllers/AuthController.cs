using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

[ApiController]
[Route("auth")]
public class AuthController(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    AppDbContext context) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedAccessException("Invalid email or password");

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7)
        });
        await context.SaveChangesAsync();

        SetRefreshTokenCookie(refreshToken);

        return Ok(new AuthResponseDto
        {
            AccessToken = accessToken,
            Email = user.Email!,
            FullName = user.FullName
        });
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh()
    {
        var oldToken = Request.Cookies["refreshToken"];
        if (oldToken is null)
            throw new UnauthorizedAccessException("Refresh token missing");

        var stored = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == oldToken);

        if (stored is null || !stored.IsActive)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        // rotation — revoke old, issue new
        stored.RevokedAt = DateTimeOffset.UtcNow;

        var roles = await userManager.GetRolesAsync(stored.User!);
        var newAccessToken = tokenService.GenerateAccessToken(stored.User!, roles);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            UserId = stored.UserId,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(7)
        });
        await context.SaveChangesAsync();

        SetRefreshTokenCookie(newRefreshToken);

        return Ok(new AuthResponseDto
        {
            AccessToken = newAccessToken,
            Email = stored.User!.Email!,
            FullName = stored.User.FullName
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        var token = Request.Cookies["refreshToken"];
        if (token is not null)
        {
            var stored = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
            if (stored is not null)
            {
                stored.RevokedAt = DateTimeOffset.UtcNow;
                await context.SaveChangesAsync();
            }
        }

        Response.Cookies.Delete("refreshToken");
        return NoContent();
    }

    private void SetRefreshTokenCookie(string token)
    {
        Response.Cookies.Append("refreshToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }
}