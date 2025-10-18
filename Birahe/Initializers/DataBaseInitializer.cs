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

        if (!_context.Users.Any(u=> u.Role == Role.Admin)) {
            _context.Users.Add(new User() {
                Username = "admin",
                Passwordhashed = "12345678".Hash(),
                Role = Role.Admin,
                TeamName = "Admin Team"
            });
        }

        _context.SaveChanges();
    }
}