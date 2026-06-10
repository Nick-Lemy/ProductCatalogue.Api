using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.DTOs;

public class UpdateProductDto
{
    [StringLength(100, MinimumLength = 2)]
    public string? Name { get; set; } = string.Empty;

    [MinLength(30)]
    public string? Description { get; set; } = string.Empty;

    [MinLength(3)]
    public string? ProductCode { get; set; } = string.Empty;

    [MinLength(3)]
    public string? Brand { get; set; } = string.Empty;

    [MinLength(3)]
    public string? Category { get; set; } = string.Empty;

    [MinLength(3)]
    public string? TargetMarket { get; set; } = string.Empty;

    [MinLength(3)]
    public string? Season { get; set; } = string.Empty;
}
