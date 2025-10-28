using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Initializers;

public class DataBaseInitializer {
    private readonly ApplicationContext _context;

    public DataBaseInitializer(ApplicationContext context) {
        _context = context;
    }


    public void SeedData() {
        _context.Database.Migrate();

        if (!_context.Users.IgnoreQueryFilters().Any(u => u.Role == Role.Admin)) {
            _context.Users.Add(new User() {
                Username = "admin",
                Passwordhashed = "12345678".Hash(),
                Role = Role.Admin,
                TeamName = "Admin Team",
            });
        }

        if (!_context.ContestConfigs.IgnoreQueryFilters().Any(cc => cc.Key == "Signup")) {
            _context.ContestConfigs.Add(new ContestConfig() {
                Key = "Signup",
                context = "sign up start time",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(15)
            });
        }

        if (!_context.ContestConfigs.IgnoreQueryFilters().Any(cc => cc.Key == "Contest")) {
            _context.ContestConfigs.Add(new ContestConfig() {
                Key = "Contest",
                context = "contest start time",
                StartTime = DateTime.UtcNow.AddDays(10),
                EndTime = DateTime.UtcNow.AddDays(20)
            });
        }

        if (!_context.ContestConfigs.IgnoreQueryFilters().Any(cc => cc.Key == "FinalContest")) {
            _context.ContestConfigs.Add(new ContestConfig() {
                Key = "FinalContest",
                context = "final contest start time",
                StartTime = DateTime.UtcNow.AddDays(20),
                EndTime = DateTime.UtcNow.AddDays(21)
            });
        }

        if (!_context.Discounts.IgnoreQueryFilters().Any()) {
            _context.Discounts.Add(new Discount() {
                Key = "Faz2025",
                Percent = 5
            });
        }


        _context.SaveChanges();
    }
}