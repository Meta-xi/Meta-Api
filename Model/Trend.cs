using System.ComponentModel.DataAnnotations;
namespace Meta_xi.Application;

public class Trend
{
    [Key]
    public int IdTendency { get; set; }
    public required string Name { get; set; }
    public required float Reward { get; set; }
    public required ICollection<TrendUser>? TrendUsers { get; set; }
}