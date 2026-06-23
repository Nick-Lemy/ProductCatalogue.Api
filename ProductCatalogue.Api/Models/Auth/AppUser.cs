using Microsoft.AspNetCore.Identity;

namespace ProductCatalogue.Api.Models;

public class AppUser: IdentityUser
{
    public required string FullName { get; set; }
}