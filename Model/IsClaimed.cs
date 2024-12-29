using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class IsClaimed
{
    [Key]
    public int IDClaimed { get; set; }
    public required int IDMission { get; set; }
    public required int UserID { get; set; }
    public required DateTime DateClaimed { get; set; }
    public required User? User { get; set; }
    public required Missions? Missions { get; set; }
}