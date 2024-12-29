using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class DisponibilityToClaim
{
    [Key]
    public int IDDisponibility { get; set; }
    public required int UserID { get; set; }
    public required float Disponibility { get; set; }
    public required User? User { get; set; }
}