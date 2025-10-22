using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Entities.Configs;
using Birahe.EndPoint.Enums;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.DataBase;

public class ApplicationContext : DbContext {
    public ApplicationContext(DbContextOptions options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfigs).Assembly);


        // ==============Relations==============

        // User to Student , one to many
        modelBuilder.Entity<User>()
            .HasMany(u => u.Students)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);

        // Riddle To ContestItem
        modelBuilder.Entity<Riddle>()
            .HasMany(u => u.ContestItems)
            .WithOne(ci => ci.Riddle)
            .HasForeignKey(ci => ci.RiddleId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // User To ContestItem
        modelBuilder.Entity<User>()
            .HasMany(u => u.ContestItems)
            .WithOne(ci => ci.User)
            .HasForeignKey(ci => ci.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        // Payment to User
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithOne(u => u.Payment)
            .HasForeignKey<Payment>(p => p.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);


        base.OnModelCreating(modelBuilder);
    }

    //=============Entities==============
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Riddle> Riddles { get; set; }
    public DbSet<ContestItem> ContestItems { get; set; }
    public DbSet<ContestConfig> ContestConfigs { get; set; }
    public DbSet<Payment> Payments { get; set; }
}