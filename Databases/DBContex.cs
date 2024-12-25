
using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options){
        
    }
    public required DbSet<User> Users { get ; set ; }
    public required DbSet<Plan> Plans { get ; set ; }
    public required DbSet<ReferLevel1> ReferLevel1s { get ; set ; }
    public required DbSet<ReferLevel2> ReferLevel2s { get ; set ; }
    public required DbSet<ReferLevel3> ReferLevel3s { get ; set ; }
    public required DbSet<Wallet> Wallets { get ; set ; }
    public required DbSet<UserPlans> UserPlans { get ; set ; }
    public required DbSet<UpdatePlansForUser> UpdatePlansForUser { get ; set ; }
    public required DbSet<BenefitPerRefer> BenefitPerRefers { get ; set ; }
    public required DbSet<RechargeLog> RechargeLogs { get ; set ; }
    public required DbSet<WithdrawLog> WithdrawLogs { get ; set ; }
    public required DbSet<Missions> Missionss { get ; set ; }
    public required DbSet<MissionsUser> MissionsUsers { get ; set ; }
    public required DbSet<Completed> Completeds { get ; set ; }
    public required DbSet<Trend> Trends { get ; set ; }
    public required DbSet<WelcomeBonus> WelcomeBonuss { get ; set ; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ReferLevel1>().HasOne(option => option.User).WithMany(option => option.referLevel1s).HasForeignKey(option=> option.IDUserReferrer);
        modelBuilder.Entity<ReferLevel2>().HasOne(option => option.User).WithMany(option => option.referLevel2s).HasForeignKey(option=> option.IDUserReferrer);
        modelBuilder.Entity<ReferLevel3>().HasOne(option => option.User).WithMany(option => option.referLevel3s).HasForeignKey(option=> option.IDUserReferrer);
        modelBuilder.Entity<RechargeLog>().HasOne(option => option.User).WithMany(option => option.rechargeLogs).HasForeignKey(option => option.IdUser);
        modelBuilder.Entity<WithdrawLog>().HasOne(u => u.User).WithMany(u => u.withdrawLogs).HasForeignKey(u => u.UserId);
        modelBuilder.Entity<MissionsUser>().HasOne(option => option.User).WithMany(option => option.missionsUSers).HasForeignKey(option => option.UserID);
        modelBuilder.Entity<MissionsUser>().HasOne(u => u.Missions).WithMany( u => u.MissionsUsers).HasForeignKey(u => u.IDMission);
        modelBuilder.Entity<WelcomeBonus>().HasOne(option => option.User).WithOne(option => option.WelcomeBonus).HasForeignKey<WelcomeBonus>(option => option.UserID);
    }

}