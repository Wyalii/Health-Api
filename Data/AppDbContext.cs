using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<BloodPanel> BloodPanel { get; set; }
    public DbSet<LipidPanel> LipidPanel { get; set; }
    public DbSet<MetabolicPanel> MetabolicPanel { get; set; }
    public DbSet<UserInfoPanel> UserInfoPanel { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserBloodPanel)
            .WithOne()
            .HasForeignKey<User>(u => u.UserBloodPanelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserLipidPanel)
            .WithOne()
            .HasForeignKey<User>(u => u.UserLipidPanelId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<User>()
            .HasOne(u => u.MetabolicPanel)
            .WithOne()
            .HasForeignKey<User>(u => u.MetabolicPanelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserInfoPanel>()
            .HasOne(uip => uip.User)
            .WithMany()
            .HasForeignKey(uip => uip.UserId)
            .OnDelete(DeleteBehavior.Cascade);


    }

}
