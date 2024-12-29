using System.ComponentModel.DataAnnotations;

namespace Meta_xi.Application;
public class User
{
    [Key]
    public int Id { get; set; }
    public required string? Email { get; set; }
    public required string? PhoneNumber { get; set; }
    public required string Password { get; set; }
    public required string Token { get ; set ;}
    public required string Code { get; set; }
    public required DateTime Date { get; set; }
    public required ICollection<ReferLevel1>? referLevel1s { get ; set ; }
    public required ICollection<ReferLevel2>? referLevel2s { get ; set ; }
    public required ICollection<ReferLevel3>? referLevel3s { get ; set ; }
    public required ICollection<RechargeLog>? rechargeLogs { get ; set ; }
    public required ICollection<WithdrawLog>? withdrawLogs { get ; set ; }
    public required Wallet? Wallet { get ; set ; }
    public required ICollection<MissionsUser>? missionsUSers { get ; set; }
    public required ICollection<TrendUser>? trendUsers { get ; set ; }
    public required WelcomeBonus? WelcomeBonus { get; set; }
    public required ICollection<DisponibilityToClaim>? DisponibilityToClaims { get; set; }
    public required ICollection<IsClaimed>? IsClaimeds { get; set; }
    
}