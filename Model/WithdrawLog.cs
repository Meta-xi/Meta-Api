using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class WithdrawLog
{
    [Key]
    public int IdWithdraw { get ; set ;}
    public required int UserId { get ; set ; }
    public required User? User { get ; set ;}
    public required DateTime Date { get ; set ; }
    public required float Withdraw { get ; set ; }

}