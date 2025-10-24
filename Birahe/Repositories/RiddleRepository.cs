using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Repositories;

public class RiddleRepository {
    private readonly DbSet<Riddle> _riddles;

    public RiddleRepository(ApplicationContext context) {
        _riddles = context.Riddles;
    }

    public async Task AddRiddle(Riddle riddle) {
        await _riddles.AddAsync(riddle);
    }

    public async Task<Riddle?> CheckExistence(string riddleUId) {
        var riddle = await _riddles.AsTracking().FirstOrDefaultAsync(r => r.RiddleUId == riddleUId);
        return riddle;
    }

    public async Task<Riddle?> FindRiddleAsync(int id) {
        return await _riddles.FirstOrDefaultAsync(r => r.Id == id);
    }

    public void EditRiddle(Riddle toEdit, Riddle riddle) {
        riddle.Adapt(toEdit);
        toEdit!.ModificationDateTime = DateTime.Now;
    }

    public void RemoveRiddle(Riddle riddle) {
        riddle!.RemoveTime = DateTime.Now;
    }

    public async Task<List<Riddle>> GetRiddles() {
        return await _riddles
            .ToListAsync();
    }
}