using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.DTOs;

public class UpdateProductDto
{
    [StringLength(100, MinimumLength = 2)]
    public string? Name { get; set; }

    public decimal? Price { get; set; }

    [StringLength(100, MinimumLength = 2)]

    public string? Description { get; set; }
}
