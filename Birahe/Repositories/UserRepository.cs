using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Repositories;

public class UserRepository {
    private ApplicationContext _context;

    public UserRepository(ApplicationContext context) {
        _context = context;
    }

    public async Task<User?> Login(string userName, string passWord) {
        var hashedPassWord = passWord.Hash();
        var user = await _context
            .Users
            .Include(u=>u.Students)
            .FirstOrDefaultAsync(
                x => x.UserName == userName && x.Passwordhashed == hashedPassWord);

        return user;
    }

    public async Task<User?> CheckExistence(string userName) {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        return user;
    }

    public async Task AddUser(User user) {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> FindUser(int id) {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
    }
}