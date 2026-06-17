using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface ITokenService
{
    string GenerateAccessToken(AppUser user, IList<string> roles);
    string GenerateRefreshToken();
}