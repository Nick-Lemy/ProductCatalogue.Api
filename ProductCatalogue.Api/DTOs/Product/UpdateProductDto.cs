using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.DTOs;

public class UpdateProductDto
{
    [StringLength(100, MinimumLength = 2)]
    public string? Name { get; set; }

    [MinLength(30)]
    public string? Description { get; set; }

    [MinLength(3)]
    public string? ProductCode { get; set; }

    [MinLength(3)]
    public string? Brand { get; set; }

    [MinLength(3)]
    public string? Category { get; set; }

    [MinLength(3)]
    public string? TargetMarket { get; set; }

    [MinLength(3)]
    public string? Season { get; set; }
}
