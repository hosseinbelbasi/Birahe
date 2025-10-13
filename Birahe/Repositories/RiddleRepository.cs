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

    public async Task<Riddle?> FindRiddleAsync(int id) {
        return await _context.Riddles.FirstOrDefaultAsync(r => r.Id == id);
    }

    public void EditRiddle(Riddle toEdit, Riddle riddle) {


        riddle.Adapt(toEdit);
        toEdit!.ModificationDateTime = DateTime.Now;
    }

    public void RemoveRiddle(Riddle riddle) {


        riddle!.RemoveTime = DateTime.Now;

    }

    public async Task<List<Riddle>> GetRiddles() {
        return await _context
            .Riddles
            .ToListAsync();
    }


}