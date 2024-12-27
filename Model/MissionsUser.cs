using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class MissionsUser
{
    [Key]
    public int IDMissionUSer { get; set; }
    public required int UserID { get; set; }
    public required int IDMission { get; set; }
    public required int Progres { get; set; }
    public required bool IsClaimed { get; set; }
    public required User? User { get; set; }
    public required Missions? Missions { get; set; }
}