using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class WelcomeBonus
{
    [Key]
    public int IDBonus { get; set; }
    public required int UserID { get; set; }
    public required User? User { get; set; }
    public required bool IsClaimed { get; set; }
}