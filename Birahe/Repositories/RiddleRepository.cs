using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Repositories;

public class RiddleRepository {
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public RiddleRepository(ApplicationContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddRiddle(Riddle riddle) {
        await _context.Riddles.AddAsync(riddle);
    }

    public async Task<Riddle?> CheckExistence(string riddleUId) {
        var riddle = await _context.Riddles.AsTracking().FirstOrDefaultAsync(r => r.RiddleUId == riddleUId);
        return riddle;
    }

    public async Task<bool> EditRiddle(string riddleUId, Riddle riddle) {
        var old = await CheckExistence(riddleUId);

        if (old == null) {
            return false;
        }

        riddle.Adapt(old);
        old.ModificationDateTime = DateTime.Now;
        return true;
    }

    public async Task<bool> RemoveRiddle(string riddleUId) {
        var riddle = await CheckExistence(riddleUId);

        if (riddle == null) {
            return false;
        }

        riddle.RemoveTime = DateTime.Now;
        return true;
    }

    public async Task<List<Riddle>> GetRiddles() {
        return await _context.Riddles.ToListAsync();
    }
}