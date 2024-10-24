using Microsoft.EntityFrameworkCore;

namespace Meta_xi.Application;
public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options){
        
    }
    public DbSet<User> Users { get ; set ; }
    public DbSet<Plan> Plans { get ; set ; }
    public DbSet<ReferLevel1> ReferLevel1s { get ; set ; }
    public DbSet<ReferLevel2> ReferLevel2s { get ; set ; }
    public DbSet<ReferLevel3> ReferLevel3s { get ; set ; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ReferLevel1>().HasOne(option => option.User).WithMany(option => option.referLevel1s).HasForeignKey(option=> option.IDUserReferrer);
        modelBuilder.Entity<ReferLevel2>().HasOne(option => option.User).WithMany(option => option.referLevel2s).HasForeignKey(option=> option.IDUserReferrer);
        modelBuilder.Entity<ReferLevel3>().HasOne(option => option.User).WithMany(option => option.referLevel3s).HasForeignKey(option=> option.IDUserReferrer);
    }

}