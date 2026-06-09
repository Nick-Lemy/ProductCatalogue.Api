using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.DTOs;

public class CreateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    [MinLength(30)]
    public string Description { get; set; } = string.Empty;
}
