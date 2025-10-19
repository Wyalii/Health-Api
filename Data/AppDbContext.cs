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
                .WithOne(bp => bp.User)        // Navigation property in BloodPanel
                .HasForeignKey<BloodPanel>(bp => bp.UserId) // Foreign key in BloodPanel
                .OnDelete(DeleteBehavior.Cascade);

        // User <-> LipidPanel
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserLipidPanel)
            .WithOne(lp => lp.User)        // Navigation property in LipidPanel
            .HasForeignKey<LipidPanel>(lp => lp.UserId) // Foreign key in LipidPanel
            .OnDelete(DeleteBehavior.Cascade);

        // User <-> MetabolicPanel
        modelBuilder.Entity<User>()
            .HasOne(u => u.MetabolicPanel)
            .WithOne(mp => mp.User)        // Navigation property in MetabolicPanel
            .HasForeignKey<MetabolicPanel>(mp => mp.UserId) // Foreign key in MetabolicPanel
            .OnDelete(DeleteBehavior.Cascade);

        // UserInfoPanel -> User (many-to-one)
        modelBuilder.Entity<UserInfoPanel>()
            .HasOne(uip => uip.User)
            .WithMany()
            .HasForeignKey(uip => uip.UserId)
            .OnDelete(DeleteBehavior.Cascade);


    }

}
