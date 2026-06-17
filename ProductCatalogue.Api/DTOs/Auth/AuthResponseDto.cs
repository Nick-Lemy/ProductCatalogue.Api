namespace ProductCatalogue.Api.DTOs;

public class AuthResponseDto
{
    public required string AccessToken { get; init; }
    public required string Email { get; init; }
    public required string FullName { get; init; }
}