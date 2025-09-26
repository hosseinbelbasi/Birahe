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
            Id = 1,
            UserName = "Admin",
            Passwordhashed = "12345678".Hash(),
            Role = Role.Admin,
            Coin = 0
        });
        base.OnModelCreating(modelBuilder);
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Riddle> Riddles { get; set; }
}