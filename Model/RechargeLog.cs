using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Meta_xi.Application;
public class RechargeLog
{
    [Key]
    public int IdRechargeLog { get; set; }
    public required int IdUser { get; set; }
    public float Recharge { get; set; }
    public DateTime Date { get; set; }
    public required User? User { get; set; }
}