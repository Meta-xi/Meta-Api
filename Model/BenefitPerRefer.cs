using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class BenefitPerRefer 
{
    [Key]
    public int IdGanancias { get ; set ; }
    public required double Nivel1 { get ; set ; }
    public required double Nivel2 { get ; set ; }
    public required double Nivel3 { get ; set ; }
    public required string Username { get ; set ;}
}