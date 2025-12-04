using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto.ContestDto_s;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Repositories;

public class ContestRepository {
    private readonly ApplicationContext _context;

    public ContestRepository(ApplicationContext context) {
        _context = context;
    }

    public async Task<ContestItem?> CheckExistence(int userId, int riddleId) {
        var ci = await _context.ContestItems.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.RiddleId == riddleId);
        return ci;
    }


    public async Task<ContestItem?> FindContestItem(int ciId) {
        return await _context.ContestItems.FirstOrDefaultAsync(ci => ci.Id == ciId);
    }

    public async Task OpenRiddleAsync(User user, Riddle riddle) {
        var ci = new ContestItem() {
            User = user,
            Riddle = riddle,
            OpeningDateTime = DateTime.UtcNow,
            Level = riddle.Level
        };

        await _context.ContestItems.AddAsync(ci);
    }

    public void OpenHint(ContestItem contestItem) {
        contestItem.HasOpenedHint = true;
        contestItem.OpeningHintDateTime = DateTime.UtcNow;
    }

    public async Task<List<RiddleWithStatusDto>?> GetAllRiddlesWithStatusAsync(int userId) {
        var query =
            from r in _context.Riddles
            join ci in _context.ContestItems
                    .Where(c => c.User.Id == userId)
                on r.Id equals ci.RiddleId into joined
            from j in joined.DefaultIfEmpty() // LEFT JOIN — includes riddles user has not opened yet
            select new RiddleWithStatusDto {
                Id = r.Id,
                Department = r.Department,
                Level = r.Level,
                No = r.No,
                Content = j != null ? r.Content : null, // only show content if opened
                IsOpened = j != null,
                IsSolved = j != null && j.IsSolved,
                HasOpenedHint = j != null && j.HasOpenedHint,
                OpeningCost = r.OpeningCost,
                HintCost = r.HintCost,
                Reward = r.Reward,
                Format = r.Format,
                HintType = $"{r.HintMediaType}".ToLower(),
                RewardType = $"{r.RewardMediaType}".ToLower(),
                InterfaceString = r.InterfaceString,
                Options = r.Options
            };

        return await query.ToListAsync();
    }

    public async Task<RiddleWithStatusDto?> GetRiddleWithStatusAsync(int riddleId, int userId) {
        var query =
            from r in _context.Riddles
            where r.Id == riddleId
            join ci in _context.ContestItems
                    .Where(ci => ci.UserId == userId)
                on r.Id equals ci.RiddleId into joined
            from j in joined.DefaultIfEmpty()
            select new RiddleWithStatusDto {
                Id = r.Id,
                Department = r.Department,
                Level = r.Level,
                No = r.No,
                Content = j != null ? r.Content : null, // only show content if opened
                IsOpened = j != null,
                IsSolved = j != null && j.IsSolved,
                HasOpenedHint = j != null && j.HasOpenedHint,
                OpeningCost = r.OpeningCost,
                HintCost = r.HintCost,
                Reward = r.Reward,
                Format = r.Format,
                HintType = $"{r.HintMediaType}".ToLower(),
                RewardType = $"{r.RewardMediaType}".ToLower(),
                InterfaceString = r.InterfaceString,
                Options = r.Options
            };

        return await query.FirstOrDefaultAsync();
    }

    public void SubmitAnswer(ContestItem ci, string answer, bool success) {
        ci.Tries += 1;
        ci.IsSolved = success;
        ci.LastAnswer = answer;
        ci.LastTryDateTime = DateTime.UtcNow;
        ci.SolvingDateTime = success ? DateTime.UtcNow : null;
        if (success) {
            ci.User.SolvedRiddles++;
        }
    }


    public async Task<List<LeaderBoardUserDto>?> GetLeaderBoardAsync() {
        var leaderBoard = await _context.Users
            .Where(u => u.Role == Role.User)
            .Select(u => new LeaderBoardUserDto() {
                TeamName = u.TeamName,
                Coin = u.Coin,
                SolvedRiddles = u.SolvedRiddles,
                UsedHintCount = u.ContestItems!.Count(ci=> ci.HasOpenedHint && ci.IsSolved) ,
                HardSolvedCount = u.ContestItems!.Count(ci=> ci.IsSolved && ci.Level == 3),
                RegularSolvedCount =u.ContestItems!.Count(ci=> ci.IsSolved && ci.Level == 2),
            }).OrderByDescending(u => u.Coin)
            .ThenByDescending(u => u.SolvedRiddles)
            .ThenBy(u=> u.UsedHintCount)
            .ThenByDescending(u=>u.HardSolvedCount).
            ThenByDescending(u=>u.RegularSolvedCount)
            .ToListAsync();


        for (int i = 0; i < leaderBoard.Count; i++) {
            leaderBoard[i].Position = i + 1;
        }

        return leaderBoard;
    }
}