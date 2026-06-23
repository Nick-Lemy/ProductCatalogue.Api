using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalogue.Api.Models;

[Index(nameof(Name), IsUnique = true)]
public class AssetTag
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    public ICollection<Asset> Assets { get; set; } = [];
}