using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.Models;

public class Product
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;

    [MinLength(30)]
    public string Description { get; set; } = "";

    public decimal Price { get; set; }

}