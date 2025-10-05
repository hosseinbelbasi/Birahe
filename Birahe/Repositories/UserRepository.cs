using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
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
            .IgnoreQueryFilters()
            .Include(u=>u.Students)
            .FirstOrDefaultAsync(
                x => x.Username == userName && x.Passwordhashed == hashedPassWord);

        return user;
    }

    public async Task<User?> CheckExistence(string userName) {
        User? user = await _context
            .Users
            .Include(u=>u.Students)
            .FirstOrDefaultAsync(x => x.Username == userName);
        return user;
    }

    public async Task AddUser(User user) {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> FindUser(int id) {
        return await _context
            .Users
            .Include(u=>u.Students)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<bool> ChangeUsername(string oldUsername, string newUsername) {
        User? user = await CheckExistence(oldUsername);
        if (user == null) {
            return false;
        }

        user.Username = newUsername;
        user.ModificationDateTime = DateTime.Now;
        return true;
    }

    public async Task<bool> ChangePassword(ChangePasswordDto changePasswordDto) {
        var user = await CheckExistence(changePasswordDto.Username);
        if (user == null || user!.Passwordhashed != changePasswordDto.OldPassword.Hash()) {
            return false;
        }

        user.Passwordhashed = changePasswordDto.NewPassword.Hash();
        return true;
    }

    public async Task<List<User>?> GetAllUser() {
        return await _context
            .Users
            .Include(u=>u.Students)
            .ToListAsync();
    }

    public async Task<bool> BanUser(BanUserDto banUserDto) {
        var user = await CheckExistence(banUserDto.Username);
        if (user == null) {
            return false;
        }

        user.IsBanned = true;
        user.BanReason = banUserDto.BsnReason;
        user.BanDateTime = DateTime.Now;
        return true;
    }
}