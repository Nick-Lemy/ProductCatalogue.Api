using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Services;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private const string RefreshTokenCookie = "refreshToken";

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await authService.LoginAsync(dto);
        SetRefreshTokenCookie(result.RefreshToken);
        return Ok(result.Response);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh()
    {
        var result = await authService.RefreshAsync(Request.Cookies[RefreshTokenCookie]);
        SetRefreshTokenCookie(result.RefreshToken);
        return Ok(result.Response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await authService.LogoutAsync(Request.Cookies[RefreshTokenCookie]);
        Response.Cookies.Delete(RefreshTokenCookie);
        return NoContent();
    }

    private void SetRefreshTokenCookie(string token)
    {
        Response.Cookies.Append(RefreshTokenCookie, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
    }
}
