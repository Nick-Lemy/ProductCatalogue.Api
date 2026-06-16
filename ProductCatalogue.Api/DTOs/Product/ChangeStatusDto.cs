using System.ComponentModel.DataAnnotations;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;

public class ChangeStatusDto
{
    [Required]
    public ProductStatus Status { get; set; }
}