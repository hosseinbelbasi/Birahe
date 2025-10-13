using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto.ContestDto_s;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services.Utilities;
using MapsterMapper;

namespace Birahe.EndPoint.Services;

public class ContestService {
    private readonly ContestRepository _contestRepository;
    private readonly UserRepository _userRepository;
    private readonly RiddleRepository _riddleRepository;
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly ImageService _imageService;

    public ContestService(ContestRepository contestRepository, UserRepository userRepository, RiddleRepository riddleRepository, ApplicationContext context, IMapper mapper, ImageService imageService) {
        _userRepository = userRepository;
        _riddleRepository = riddleRepository;
        _context = context;
        _mapper = mapper;
        _imageService = imageService;
        _contestRepository = contestRepository;
    }

    public async Task<ServiceResult<List<RiddleWithStatusDto>>> GetAllRiddlesWithStatusAsync(string username)
    {
        var riddles = await _contestRepository.GetAllRiddlesWithStatusAsync(username);

        if (riddles == null || riddles.Count == 0)
        {
            return ServiceResult<List<RiddleWithStatusDto>>.NoContent();
        }

        return ServiceResult<List<RiddleWithStatusDto>>.Ok(riddles, "معماها با موفقیت دریافت شدند.");
    }

    public async Task<ServiceResult<RiddleWithStatusDto>> GetRiddleWithStatusAsync(int riddleId, string username) {
        var user = await _userRepository.CheckExistence(username);
        var riddleWithStatus = await _contestRepository.GetRiddleWithStatusAsync(riddleId, user.Id);
        if (riddleWithStatus == null) {
            return ServiceResult<RiddleWithStatusDto>.Fail("این معما وجود ندارد", ErrorType.NotFound);
        }

        var riddleWithStatusDto = _mapper.Map<RiddleWithStatusDto>(riddleWithStatus);
        return ServiceResult<RiddleWithStatusDto>.Ok(riddleWithStatusDto);
    }

    public async Task<ServiceResult<ContestRiddleDto>> OpenRiddleAsync(string username,int id) {
        var user = await _userRepository.CheckExistence(username);

        // var riddleUId = openRiddleDto.Department + openRiddleDto.No;
        var riddle = await _riddleRepository.FindRiddleAsync(id);

        if (riddle==null) {
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
        return ServiceResult<ContestRiddleDto>.Ok( riddleDto,"معما با موفقیت باز شد!");
    }

    public async Task<ServiceResult<OpenHintDto>> OpenRiddleHintAsync(string username, int id) {
        var user = await _userRepository.CheckExistence(username);

        // var riddleUId = openRiddleDto.Department + openRiddleDto.No;
        var riddle = await _riddleRepository.FindRiddleAsync(id);

        if (riddle==null) {
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

        return ServiceResult<OpenHintDto>.Ok( riddleDto,"راهنمایی معما با موفقیت باز شد!");
    }

    public async Task<ServiceResult> SubmitAnswerAsync(string username, SubmitAnswerDto submitAnswerDto) {
        var user = await _userRepository.CheckExistence(username);
        var riddle = await _riddleRepository.FindRiddleAsync(submitAnswerDto.riddleId);

        if (riddle==null) {
            return ServiceResult.Fail("این معما وجود ندارد!");
        }

        var ciExists = await _contestRepository.CheckExistence(user!.Id, riddle.Id);
        if (ciExists == null) {
            return ServiceResult.Fail("این معما را هنوز باز نکرده اید.");
        }

        if (ciExists.IsSolved) return ServiceResult.Fail("شما قبلا این معما را حل کرده اید!");

        var success = riddle.Asnwer == submitAnswerDto.Answer;
        _contestRepository.SubmitAnswer(ciExists,submitAnswerDto.Answer ,success);

        if (!success) {
            return ServiceResult.Fail("متاسفانه جواب نا درست بود !");
        }


        _userRepository.IncreaseBalance(user,riddle.Reward);
        var rows =await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("خطا در ثبت جواب معما!" , ErrorType.ServerError);
        }
        return ServiceResult.Ok( "معما با موفقیت حل شد!");
    }

    public async Task<ServiceResult<List<LeaderBoardUserDto>>> GetLeaderBoardAsync() {
        var leaderBoard = await _contestRepository.GetLeaderBoardAsync();
        if (leaderBoard == null || leaderBoard.Count == 0) {
            return ServiceResult<List<LeaderBoardUserDto>>.NoContent();
        }

        return ServiceResult<List<LeaderBoardUserDto>>.Ok(leaderBoard);
    }

    // ======================== Image Retrieval ==========================

    public async Task<ServiceResult<(byte[] Bytes, string ContentType)>> GetRewardImageAsync(string username, int riddleId)
    {
        // 1️⃣ Check user existence
        var user = await _userRepository.CheckExistence(username);
        if (user == null)
            return ServiceResult<(byte[], string)>.Fail("کاربر یافت نشد!", ErrorType.NotFound);

        // 2️⃣ Check if user has solved this riddle
        var ci = await _contestRepository.CheckExistence(user.Id, riddleId);
        if (ci == null || !ci.IsSolved)
            return ServiceResult<(byte[], string)>.Fail("کاربر اجازه دسترسی ندارد!", ErrorType.Forbidden);

        // 3️⃣ Get riddle
        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);
        if (riddle == null || string.IsNullOrEmpty(riddle.RewardImageFileName))
            return ServiceResult<(byte[], string)>.Fail("عکس یافت نشد!", ErrorType.NotFound);

        // 4️⃣ Read image securely via ImageService
        var imageResult = await _imageService.ReadImageAsync(riddle.RewardImageFileName);
        if (!imageResult.Success)
            return ServiceResult<(byte[], string)>.Fail(imageResult.Message== null ?"" : imageResult.Message, imageResult.Error);

        // 5️⃣ Return image bytes and content type
        return ServiceResult<(byte[], string)>.Ok(imageResult.Data);
    }

    public async Task<ServiceResult<(byte[] Bytes, string ContentType)>> GetHintImageAsync(string username,
        int riddleId) {
        var user = await _userRepository.CheckExistence(username);
        if (user == null)
            return ServiceResult<(byte[], string ContentType)>.Fail("کاربر یافت نشد!", ErrorType.NotFound);

        var ci = await _contestRepository.CheckExistence(user.Id, riddleId);
        if (ci == null || !ci.HasOpenedHint)
            return ServiceResult<(byte[], string ContentType)>.Fail("کاربر اجازه دسترسی ندارد!", ErrorType.Forbidden);

        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);
        if (riddle == null || string.IsNullOrEmpty(riddle.HintImageFileName))
            return ServiceResult<(byte[], string ContentType)>.Fail("عکس یافت نشد!", ErrorType.NotFound);

        var imageResult = await _imageService.ReadImageAsync(riddle.HintImageFileName);
        if (!imageResult.Success)
            return ServiceResult<(byte[], string ContentType)>.Fail(imageResult.Message== null ?"" : imageResult.Message, imageResult.Error);

        return ServiceResult<(byte[], string ContentType)>.Ok(imageResult.Data);
    }

}