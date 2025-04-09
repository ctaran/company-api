using System.ComponentModel.DataAnnotations;

namespace CompanyApi.Models;

public class Company
{
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string StockTicker { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Exchange { get; set; } = string.Empty;

    [Required]
    [MaxLength(12)]
    public string Isin { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Website { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
} 