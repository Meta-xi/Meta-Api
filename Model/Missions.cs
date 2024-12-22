using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class Missions
{
    [Key]
    public int IDMission { get; set; }
    public required string Name { get; set; }
    public required int Progres { get; set; }
    public required int Goal { get; set; } 
    public required bool IsClaimed { get; set; }
    public required float Reward { get; set; }
    public required ICollection<MissionsUser> MissionsUsers { get; set; }

}