using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Birahe.EndPoint.Constants.Enums;
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

    public async Task<User?> Login(string username, string password) {
        var hashedPassWord = password.Hash();
        var user = await _context
            .Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(u => u.Students)
            .FirstOrDefaultAsync(x => x.Username == username && x.Passwordhashed == hashedPassWord);


        return user;
    }

    public async Task<User?> CheckExistence(string username) {
        User? user = await _context
            .Users
            .AsTracking()
            .Include(u => u.Students)
            .FirstOrDefaultAsync(x => x.Username == username);
        return user;
    }

    public async Task<User?> CheckExistenceSignup(string username, string teamName) {
        User? user = await _context
            .Users
            .IgnoreQueryFilters()
            .Include(u => u.Students)
            .FirstOrDefaultAsync(x => x.Username == username || x.TeamName == teamName);
        return user;
    }

    public async Task AddUser(User user) {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> FindUser(int id) {
        return await _context
            .Users
            .Include(u => u.Students)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User?> FindNotActiveUser(int id) {
        return await _context
            .Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public void Update(User user) {
        user.ModificationDateTime = DateTime.Now;
        _context.Users.Update(user);
    }

    public async Task<User?> FindBannedUser(int id) {
        return await _context.Users.AsNoTracking().IgnoreQueryFilters()
            .Where(u => u.IsBanned && u.RemoveTime.HasValue == false)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public void ChangeUsername(User user, string newUsername) {
        user!.Username = newUsername;
        user.ModificationDateTime = DateTime.Now;

        Update(user);
    }

    public bool ChangePassword(User user, string oldPassword, string newPassword) {
        if (user!.Passwordhashed != oldPassword.Hash()) {
            return false;
        }

        user.Passwordhashed = newPassword.Hash();
        Update(user);
        return true;
    }

    public async Task<List<User>?> GetAllUser() {
        return await _context
            .Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(u => u.Students)
            .ToListAsync();
    }

    public void BanUser(User user, string banReason) {
        user.IsBanned = true;
        user.BanReason = banReason;
        user.BanDateTime = DateTime.Now;

        Update(user);
    }

    public void AdminChangePassword(User user, string password) {
        user.Passwordhashed = password.Hash();

        Update(user);
    }

    public void IncreaseBalance(User user, int c) {
        user.Coin += c;

        Update(user);
    }

    public bool DecreaseBalance(User user, int c) {
        if (user.Coin < c) {
            return false;
        }

        user.Coin -= c;

        Update(user);
        return true;
    }

    public void UnBanUser(User user) {
        user.IsBanned = false;
        user.BanReason = null;
        user.BanDateTime = null;

        Update(user);
    }

    public int GetBalance(User user) {
        return user.Coin;
    }

    public async Task<List<ContestItem>?> AdminGetUserStausAsync(int userId) {
        return await _context.ContestItems
            .AsNoTracking()
            .Where(ci => ci.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> ActivateUser(int userId)
    {
        var user = await FindNotActiveUser(userId);

        if (user == null)
        {
            Console.WriteLine("log 2: user not found");
            return false;
        }

        user.IsActive = true;
        Update(user); // your repository method to mark entity modified
        await _context.SaveChangesAsync(); // persist change immediately
        return true;
    }


}