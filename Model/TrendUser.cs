using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class TrendUser
{
    [Key]
    public int IdTrendUser { get; set; }
    public required int IDTrend { get; set; }
    public required int IdUser { get; set; }
    public required bool IsClaimed { get; set; }
    public required User? User { get; set; }
    public required Trend? Trend { get; set; }
}