using System;
using System.Linq;
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
                Username = "ceo_nima",
                Passwordhashed = "GRgsDiWR9dc3QE6".Hash(),
                Role = Role.Admin,
                TeamName = "Admin Team",

            });
        }

        if (!_context.ContestConfigs.IgnoreQueryFilters().Any(cc => cc.Key == "Signup")) {
            _context.ContestConfigs.Add(new ContestConfig() {
                Key = "Signup",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(5),
                message = " ثبت نام "

            });
        }


        if (!_context.ContestConfigs.IgnoreQueryFilters().Any(cc => cc.Key == "Contest")) {
            _context.ContestConfigs.Add(new ContestConfig() {
                Key = "Contest",
                StartTime = DateTime.UtcNow.AddDays(7),
                EndTime = DateTime.UtcNow.AddDays(20),
                message = " مسابقه "
            });
        }

        if (!_context.ContestConfigs.IgnoreQueryFilters().Any(cc => cc.Key == "FinalContest")) {
            _context.ContestConfigs.Add(new ContestConfig() {
                Key = "FinalContest",
                StartTime = DateTime.UtcNow.AddDays(20),
                EndTime = DateTime.UtcNow.AddDays(21),
                message = " مسابقه نهایی "
            });
        }

        if (!_context.Discounts.IgnoreQueryFilters().Any(d=> d.Key =="Faz2025")) {
            _context.Discounts.Add(new Discount() {
                Key = "Faz2025",
                Percent = 5,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            });
        }

        if (!_context.Discounts.IgnoreQueryFilters().Any(d=> d.Key =="BiraheOpening")) {
            _context.Discounts.Add(new Discount() {
                Key = "BiraheOpening",
                Percent = 25,
                ExpiresAt = DateTime.UtcNow.AddDays(30)

                // ExpiresAt = new DateTime(2025, 11, 4, 8, 30, 0, DateTimeKind.Utc)
            });
        }


        _context.SaveChanges();
    }
}