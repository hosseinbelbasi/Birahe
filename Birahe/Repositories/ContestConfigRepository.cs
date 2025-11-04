using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Repositories;

public class ContestConfigRepository {
    private readonly ApplicationContext _context;

    public ContestConfigRepository(ApplicationContext context) {
        _context = context;
    }

    public async Task<ContestConfig?> CheckExistence(string key) {
        return await _context.ContestConfigs.FirstOrDefaultAsync(cc => cc.Key == key);
    }

    public async Task AddContestConfig(ContestConfig contestConfig) {
        await _context.ContestConfigs.AddAsync(contestConfig);
    }

    public void UpdateContestConfig(ContestConfig contestConfig) {
        _context.ContestConfigs.Update(contestConfig);
    }

    public async Task<List<ContestConfig>> GetAllConfigs() {
        return await _context.ContestConfigs.ToListAsync();
    }
}