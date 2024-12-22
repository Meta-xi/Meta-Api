using System.ComponentModel.DataAnnotations;

public class Trend
{
    [Key]
    public int IdTendency { get; set; }
    public required string Name { get; set; }
    public required int Progres { get; set; }
    public required int Goal { get; set; }
    public required bool IsClaimed { get; set; }
    public required float Reward { get; set; }
}