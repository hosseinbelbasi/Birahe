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
        modelBuilder.Entity<User>().HasData(new User {
            Id = 100,
            Username = "Admin",
            Passwordhashed = "12345678".Hash(),
            Role = Role.Admin,
            Coin = 0
        });

        // ==============Relations==============

        // User to Student , one to many
        modelBuilder.Entity<User>()
            .HasMany(u => u.Students)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);

        // ContestItem to User , one to many
        modelBuilder.Entity<ContestItem>()
            .HasOne(ci => ci.User)
            .WithMany(u => u.ContestItems)
            .HasForeignKey(ci => ci.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        // ContestItem to Riddle , ont to many
        modelBuilder.Entity<ContestItem>()
            .HasOne(ci => ci.Riddle)
            .WithMany(r => r.ContestItems)
            .HasForeignKey(ci => ci.RiddleId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }

    //=============Entities==============
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Riddle> Riddles { get; set; }
    public DbSet<ContestItem> ContestItems { get; set; }

    public DbSet<ContestConfig> ContestConfigs { get; set; }





}