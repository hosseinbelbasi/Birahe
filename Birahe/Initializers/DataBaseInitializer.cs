using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;

namespace Birahe.EndPoint.Initializers;

public class DataBaseInitializer {
    public class DatabaseInitializer
    {
        private ApplicationContext _context;

        public DatabaseInitializer(ApplicationContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            if (!_context.Users.Any())
            {
                _context.Users.Add(new User()
                {
                    UserName = "admin",
                    Passwordhashed =  "12345678".Hash(),
                    Role = Role.Admin,


                });
            }

            _context.SaveChanges();
        }
    }
}