using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Extensions;
using ProductCatalogue.Api.Services;

namespace ProductCatalogue.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    private const string RefreshTokenCookie = "refreshToken";

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.IsSuccess)
            return Unauthorized(result.Error!.Message);

        SetRefreshTokenCookie(result.Value!.RefreshToken);
        return Ok(result.Value.Response);
    }

    [Authorize]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh()
    {
        var result = await _authService.RefreshAsync(Request.Cookies[RefreshTokenCookie]);
        if (!result.IsSuccess)
            return Unauthorized(result.Error!.Message);

        SetRefreshTokenCookie(result.Value!.RefreshToken);
        return Ok(result.Value.Response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var result = await _authService.LogoutAsync(Request.Cookies[RefreshTokenCookie]);
        Response.Cookies.Delete(RefreshTokenCookie);
        return result.ToActionResult();
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
