using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class DatesBack
{
    public required int QuantityMisionsToday { get; set; }
    public required int QuantityMisions { get; set; }
    public required float Disponibility { get; set; }
}