using Birahe.EndPoint.Caching;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto.ContestDto_s;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services.Utilities;
using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;
using Birahe.EndPoint.Constants;

namespace Birahe.EndPoint.Services;

public class ContestService {
    private readonly ContestRepository _contestRepository;
    private readonly UserRepository _userRepository;
    private readonly RiddleRepository _riddleRepository;
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly MediaService _mediaService;
    private readonly MemoryCacheService _cacheService;


    public ContestService(ContestRepository contestRepository, UserRepository userRepository,
        RiddleRepository riddleRepository, ApplicationContext context, IMapper mapper, MediaService mediaService,
        MemoryCacheService cacheService) {
        _userRepository = userRepository;
        _riddleRepository = riddleRepository;
        _context = context;
        _mapper = mapper;
        _mediaService = mediaService;
        _cacheService = cacheService;
        _contestRepository = contestRepository;
    }

    public async Task<ServiceResult<AllRiddlesWithStatusDto>> GetAllRiddlesWithStatusAsync(int userId) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult<AllRiddlesWithStatusDto>.Fail("کاربریافت نشد!", ErrorType.NotFound);
        }

        var result = new AllRiddlesWithStatusDto() {
            riddles = await _contestRepository.GetAllRiddlesWithStatusAsync(userId),
            Solved = user.SolvedRiddles
        };


        if (result.riddles == null || result.riddles.Count == 0) {
            return ServiceResult<AllRiddlesWithStatusDto>.NoContent();
        }

        return ServiceResult<AllRiddlesWithStatusDto>.Ok(result, "معماها با موفقیت دریافت شدند.");
    }

    public async Task<ServiceResult<RiddleWithStatusDto>> GetRiddleWithStatusAsync(int userId, int riddleId) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult<RiddleWithStatusDto>.Fail("کاربریافت نشد!", ErrorType.NotFound);
        }

        var riddleWithStatus = await _contestRepository.GetRiddleWithStatusAsync(riddleId, user.Id);
        if (riddleWithStatus == null) {
            return ServiceResult<RiddleWithStatusDto>.Fail("شما این معما را باز نکرده اید!", ErrorType.Forbidden);
        }

        var riddleWithStatusDto = _mapper.Map<RiddleWithStatusDto>(riddleWithStatus);
        return ServiceResult<RiddleWithStatusDto>.Ok(riddleWithStatusDto);
    }

    public async Task<ServiceResult<ContestRiddleDto>> OpenRiddleAsync(int userId, int riddleId) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult<ContestRiddleDto>.Fail("کاربریافت نشد!", ErrorType.NotFound);
        }

        // var riddleUId = openRiddleDto.Department + openRiddleDto.No;
        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);

        if (riddle == null) {
            return ServiceResult<ContestRiddleDto>.Fail("این معما وجود ندارد!");
        }

        var ciExists = await _contestRepository.CheckExistence(user.Id, riddle.Id);
        if (ciExists != null) {
            return ServiceResult<ContestRiddleDto>.Fail("این معما را قبلا باز کرده اید.");
        }

        if (user.Coin < riddle.OpeningCost) {
            return ServiceResult<ContestRiddleDto>.Ok(null, "موجودی شما برای باز کردن معما کافی نیست!");
        }

        await _contestRepository.OpenRiddleAsync(user, riddle);
        var riddleDto = _mapper.Map<ContestRiddleDto>(riddle);
        var rows = await _context.SaveChangesAsync();

        if (rows == 0) {
            return ServiceResult<ContestRiddleDto>.Fail("خطا در باز کردت معما!", ErrorType.ServerError);
        }

        return ServiceResult<ContestRiddleDto>.Ok(riddleDto, "معما با موفقیت باز شد!");
    }

    public async Task<ServiceResult<OpenHintDto>> OpenRiddleHintAsync(int userId, int riddleId) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult<OpenHintDto>.Fail("کاربریافت نشد!", ErrorType.NotFound);
        }

        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);

        if (riddle == null) {
            return ServiceResult<OpenHintDto>.Fail("این معما وجود ندارد!");
        }

        var ciExists = await _contestRepository.CheckExistence(user.Id, riddle.Id);
        if (ciExists == null) {
            return ServiceResult<OpenHintDto>.Fail("این معما را هنوز باز نکرده اید.");
        }

        if (user.Coin < riddle.HintCost) {
            return ServiceResult<OpenHintDto>.Ok(data: null, "موجودی شما برای باز کردن راهنمایی معما کافی نیست!");
        }

        _userRepository.DecreaseBalance(user, riddle.HintCost);
        _contestRepository.OpenHint(ciExists);
        var rows = await _context.SaveChangesAsync();

        if (rows == 0) {
            return ServiceResult<OpenHintDto>.Fail("خطا در باز کردت راهنمایی معما!");
        }

        var riddleDto = _mapper.Map<OpenHintDto>(riddle);

        return ServiceResult<OpenHintDto>.Ok(riddleDto, "راهنمایی معما با موفقیت باز شد!");
    }

    public async Task<ServiceResult> SubmitAnswerAsync(int userId, int riddleId, SubmitAnswerDto submitAnswerDto) {
        var user = await _userRepository.FindUser(userId);
        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);

        if (riddle == null) {
            return ServiceResult.Fail("این معما وجود ندارد!");
        }

        var ciExists = await _contestRepository.CheckExistence(user!.Id, riddle.Id);
        if (ciExists == null) {
            return ServiceResult.Fail("این معما را هنوز باز نکرده اید.");
        }

        if (ciExists.IsSolved) return ServiceResult.Fail("شما قبلا این معما را حل کرده اید!");

        // rate limiting for submitting answers

        var minInterval = TimeSpan.FromMinutes(5);
        if (ciExists.LastTryDateTime.HasValue && DateTime.UtcNow - ciExists.LastTryDateTime.Value < minInterval) {
            return ServiceResult.Fail(
                $"لطفا قبل از ارسال جواب بعدی {minInterval.TotalSeconds} دقیقه صبر کنید."
            );
        }

        ciExists.LastTryDateTime = DateTime.UtcNow;
        ciExists.Tries += 1;
        ciExists.LastAnswer = submitAnswerDto.Answer;

        // end of rate limit

        var success = riddle.Answer == submitAnswerDto.Answer;
        _contestRepository.SubmitAnswer(ciExists, submitAnswerDto.Answer, success);

        if (!success) {
            return ServiceResult.Fail("متاسفانه جواب نا درست بود !");
        }


        _userRepository.IncreaseBalance(user, riddle.Reward);
        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("خطا در ثبت جواب معما!", ErrorType.ServerError);
        }

        _cacheService.Remove(CacheKeys.Leaderboard);
        return ServiceResult.Ok("معما با موفقیت حل شد!");
    }

    public async Task<ServiceResult<List<LeaderBoardUserDto>>> GetLeaderBoardAsync() {
        var leaderBoard = await _cacheService.GetOrSetAsync(
            CacheKeys.Leaderboard,
            async () => await _contestRepository.GetLeaderBoardAsync(),
            TimeSpan.FromMinutes(5)
        );


        if (leaderBoard == null || leaderBoard.Count == 0) {
            return ServiceResult<List<LeaderBoardUserDto>>.NoContent();
        }

        return ServiceResult<List<LeaderBoardUserDto>>.Ok(leaderBoard);
    }

    public async Task<ServiceResult<int>> GetUserBalanceAsync(int userId) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult<int>.Fail("کاربر یافت نشد!");
        }

        var balance = _userRepository.GetBalance(user);
        return ServiceResult<int>.Ok(balance);
    }


    // ======================== Image Retrieval ==========================

    public async Task<ServiceResult<(byte[] Bytes, string ContentType)>> GetRewardMediaAsync(int userId, int riddleId) {
        var user = await _userRepository.FindUser(userId);
        if (user == null)
            return ServiceResult<(byte[], string)>.Fail("کاربر یافت نشد!", ErrorType.NotFound);


        var ci = await _contestRepository.CheckExistence(user.Id, riddleId);
        if (ci == null || !ci.IsSolved)
            return ServiceResult<(byte[], string)>.Fail("کاربر اجازه دسترسی ندارد!", ErrorType.Forbidden);


        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);
        if (riddle == null || string.IsNullOrEmpty(riddle.RewardFileName))
            return ServiceResult<(byte[], string)>.Fail("عکس یافت نشد!", ErrorType.NotFound);


        var imageResult = await _mediaService.ReadFileAsync(riddle.RewardFileName);
        if (!imageResult.Success)
            return ServiceResult<(byte[], string)>.Fail(imageResult.Message == null ? "" : imageResult.Message,
                imageResult.Error);


        return ServiceResult<(byte[], string)>.Ok(imageResult.Data);
    }

    public async Task<ServiceResult<(byte[] Bytes, string ContentType)>> GetHintMediaAsync(int userId,
        int riddleId) {
        var user = await _userRepository.FindUser(userId);
        if (user == null)
            return ServiceResult<(byte[], string ContentType)>.Fail("کاربر یافت نشد!", ErrorType.NotFound);

        var ci = await _contestRepository.CheckExistence(user.Id, riddleId);
        if (ci == null || !ci.HasOpenedHint)
            return ServiceResult<(byte[], string ContentType)>.Fail("کاربر اجازه دسترسی ندارد!", ErrorType.Forbidden);

        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);
        if (riddle == null || string.IsNullOrEmpty(riddle.HintFileName))
            return ServiceResult<(byte[], string ContentType)>.Fail("عکس یافت نشد!", ErrorType.NotFound);

        var imageResult = await _mediaService.ReadFileAsync(riddle.HintFileName);
        if (!imageResult.Success)
            return ServiceResult<(byte[], string ContentType)>.Fail(
                imageResult.Message == null ? "" : imageResult.Message, imageResult.Error);

        return ServiceResult<(byte[], string ContentType)>.Ok(imageResult.Data);
    }
}