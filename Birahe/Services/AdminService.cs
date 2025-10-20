using Birahe.EndPoint.Caching;
using Birahe.EndPoint.Constants;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services.Utilities;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Birahe.EndPoint.Services;

public class AdminService {
    private readonly RiddleRepository _riddleRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationContext _context;
    private readonly UserRepository _userRepository;
    private readonly ImageService _imageService;
    private readonly ContestConfigRepository _configRepository;
    private readonly MemoryCacheService _cacheService;

    public AdminService(
        RiddleRepository riddleRepository,
        IMapper mapper,
        ApplicationContext context,
        UserRepository userRepository,
        ImageService imageService, ContestConfigRepository configRepository, MemoryCacheService cacheService) {
        _riddleRepository = riddleRepository;
        _mapper = mapper;
        _context = context;
        _userRepository = userRepository;
        _imageService = imageService;
        _configRepository = configRepository;
        _cacheService = cacheService;
    }


    // ================Riddle business=============
    public async Task<ServiceResult> AddRiddleAsync(AddRiddleDto addRiddleDto) {
        var riddleDtoUId = addRiddleDto.Department + addRiddleDto.No;
        var exists = await _riddleRepository.CheckExistence(riddleDtoUId);
        if (exists != null) {
            return ServiceResult.Fail("این معما قبلا ثبت شده است!");
        }

        Riddle riddle = _mapper.Map<Riddle>(addRiddleDto);
        await _riddleRepository.AddRiddle(riddle);
        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("ثبت معما با خطا روبرو شد!", ErrorType.ServerError);
        }

        _cacheService.Remove(CacheKeys.AdminAllRiddles);
        return ServiceResult.Ok("معما با موفقیت ثبت شد!");
    }

    public async Task<ServiceResult<AdminRiddleDto>> EditRiddleAsync(int riddleId, AdminRiddleDto adminRiddleDto) {
        var exists = await _riddleRepository.FindRiddleAsync(riddleId);
        if (exists == null) {
            return ServiceResult<AdminRiddleDto>.Fail("این معما قبلا ثبت نشده است.");
        }


        var oldRiddle = _mapper.Map<Riddle>(adminRiddleDto);

        _riddleRepository.EditRiddle(exists, oldRiddle);


        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult<AdminRiddleDto>.Fail("ویرایش معما با خطا رو به رو شد!", ErrorType.ServerError);
        }
        _cacheService.Remove(CacheKeys.AdminAllRiddles);

        return ServiceResult<AdminRiddleDto>.Ok(adminRiddleDto, message: "معما با موفقیت ویرایش شد.");
    }

    public async Task<ServiceResult<List<AdminRiddleDto>>> GetAllRiddlesAsync() {
        var riddles = await _cacheService.GetOrSetAsync(
            CacheKeys.AdminAllRiddles,
            async () => await _riddleRepository.GetRiddles(),
            TimeSpan.FromMinutes(60)
        );

        if (riddles.Count == 0) {
            return ServiceResult<List<AdminRiddleDto>>.NoContent();
        }

        var riddlesDto = _mapper.Map<List<AdminRiddleDto>>(riddles);

        return ServiceResult<List<AdminRiddleDto>>.Ok(riddlesDto, "خدمت شما.");
    }

    public async Task<ServiceResult> DeleteRiddleAsync(int riddleId) {

        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);
        if (riddle == null) {
            return ServiceResult.Fail("این معما قبلا ثبت نشده است.");
        }

        _riddleRepository.RemoveRiddle(riddle);

        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("حدف معما با خطا رو به رو شد!", ErrorType.ServerError);
        }

        _cacheService.Remove(CacheKeys.AdminAllRiddles);

        return ServiceResult.Ok("معما با موفقیت حذف شد");
    }

    public async Task<ServiceResult<AdminRiddleDto>> GetRiddleByIdAsync(int id) {
        var riddle = await _riddleRepository.FindRiddleAsync(id);
        if (riddle == null) {
            return ServiceResult<AdminRiddleDto>.Fail("این معما قبلا ثبت نشده است.");
        }

        var riddleDto = _mapper.Map<AdminRiddleDto>(riddle);
        return ServiceResult<AdminRiddleDto>.Ok(riddleDto);
    }

    public async Task<ServiceResult>
        UploadRiddleImageAsync(int riddleId, IFormFile? hintImage, IFormFile? rewardImage) {
        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);
        if (riddle == null) {
            return ServiceResult.Fail("معما یافت نشد!", ErrorType.NotFound);
        }

        if (hintImage == null && rewardImage == null) {
            return ServiceResult.Fail("هیچ عکسی آپلود نکرده اید!");
        }

        if (hintImage != null) {
            var imgResult = await _imageService.SaveImageAsync(hintImage);
            if (!imgResult.Success) {
                return ServiceResult.Fail(imgResult.Message, imgResult.Error);
            }

            riddle.HintImageFileName = imgResult.Data;
        }

        if (rewardImage != null) {
            var imgResult = await _imageService.SaveImageAsync(rewardImage);
            if (!imgResult.Success) {
                return ServiceResult.Fail(imgResult.Message, imgResult.Error);
            }

            riddle.RewardImageFileName = imgResult.Data;
        }

        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("دخیره عکس با خطا مواجه شد!", ErrorType.ServerError);
        }

        _cacheService.Remove(CacheKeys.AdminAllRiddles);

        return ServiceResult.Ok("عکس(ها) با موفقیت ذخیره شدند!");
    }


    public async Task<ServiceResult<(byte[] File, string ContentType)>> GetRiddleImageByIdAsync(int riddleId,
        string type) {
        // 1️⃣ Find the riddle
        var riddle = await _context.Riddles.FindAsync(riddleId);
        if (riddle == null)
            return ServiceResult<(byte[], string)>.Fail("معما پیدا نشد!", ErrorType.NotFound);

        // 2️⃣ Determine which image to load
        string? fileName = type.ToLower() switch {
            "hint" => riddle.HintImageFileName,
            "reward" => riddle.RewardImageFileName,
            _ => null
        };

        if (string.IsNullOrEmpty(fileName))
            return ServiceResult<(byte[], string)>.Fail("عکس یافت نشد!", ErrorType.NotFound);

        // 3️⃣ Read the image securely
        var imageResult = await _imageService.ReadImageAsync(fileName);
        if (!imageResult.Success)
            return ServiceResult<(byte[], string)>.Fail(imageResult.Message ?? "خطا در خواندن تصویر!",
                imageResult.Error);

        // 4️⃣ Return image bytes and content type
        return ServiceResult<(byte[], string)>.Ok(imageResult.Data);
    }


    private static string GetContentType(string path) {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext switch {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }


    // =======================User business===================

    public async Task<ServiceResult<List<AdminUserDto>>> GetAllUsersAsync() {
        var userList = await _cacheService.GetOrSetAsync(
            CacheKeys.AdminAllUsers,
            async ()=> await _userRepository.GetAllUser(),
            TimeSpan.FromMinutes(10)
            );
        if (userList == null) {
            return ServiceResult<List<AdminUserDto>>.NoContent();
        }

        var userListDto = _mapper.Map<List<AdminUserDto>>(userList);

        return ServiceResult<List<AdminUserDto>>.Ok(userListDto, "خدمت شما");
    }


    public async Task<ServiceResult> BanUserAsync(int userId ,BanUserDto banUserDto) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }

        _userRepository.BanUser(user, banUserDto.BsnReason);


        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("فرآیند بن کردن با خطا رو به رو شد!", ErrorType.ServerError);
        }

        _cacheService.Remove(CacheKeys.AdminAllUsers);

        return ServiceResult.Ok("کاربر با موفقیت بن شد.");
    }

    public async Task<ServiceResult> UnBanUserAsync(int userId) {
        var user = await _userRepository.FindBannedUser(userId);
        if (user == null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }

        _userRepository.UnBanUser(user);


        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("فرآیند رفع مسدود کردن با خطا رو به رو شد!", ErrorType.ServerError);
        }
        _cacheService.Remove(CacheKeys.AdminAllUsers);

        return ServiceResult.Ok("کاربر با موفقیت رفع مسدود شد.");
    }


    public async Task<ServiceResult> EditUserUsernameAsync(int userId,AdminEditUserUsernameDto usernameDto) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }

        _userRepository.ChangeUsername(user, usernameDto.NewUsername);

        var rows = await _context.SaveChangesAsync();

        if (rows == 0) {
            return ServiceResult.Fail("تغییر نام کاربری با خطا مواجه شد!");
        }
        _cacheService.Remove(CacheKeys.AdminAllUsers);

        return ServiceResult.Ok("نام کاربری با موفقیت تغییر یافت.");
    }

    public async Task<ServiceResult> EditUserPasswordAsync(int userId, AdminEditUserPasswordDto userPasswordDto) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }


        _userRepository.AdminChangePassword(user, userPasswordDto.NewPassword);

        var rows = await _context.SaveChangesAsync();

        if (rows == 0) {
            return ServiceResult.Fail("تغییر رمز عبور با خطا مواجه شد!");
        }
        _cacheService.Remove(CacheKeys.AdminAllUsers);

        return ServiceResult.Ok("رمز عبور با موفقیت تغییر یافت.");
    }

    public async Task<ServiceResult<List<AdminContestItemDto>?>> GetUserStatusAsync(int userId) {
        var list = await _userRepository.AdminGetUserStausAsync(userId);

        if (list == null || list.Count == 0)
            return ServiceResult<List<AdminContestItemDto>?>.Fail("برای این کاربر هیچ سابقه‌ای یافت نشد.",
                ErrorType.NotFound);

        var listDto = _mapper.Map<List<AdminContestItemDto>>(list);


        return ServiceResult<List<AdminContestItemDto>?>.Ok(listDto);
    }

    // ====================Contest Config =======================//

    public async Task<ServiceResult<DateTime>> SetContestStartTimeAsync( SetContestStartDto setContestStartDto) {
        var config = await _context.ContestConfigs.FirstOrDefaultAsync(x => x.Key == setContestStartDto.Key);
        if (config == null) {
            config = new ContestConfig { Key = setContestStartDto.Key, StartTime = setContestStartDto.StartTime };
            _context.ContestConfigs.Add(config);
        }
        else {
            config.StartTime = setContestStartDto.StartTime;
            _context.ContestConfigs.Update(config);
        }

        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult<DateTime>.Fail("خطا در ثبت عملیات", ErrorType.ServerError);
        }

        return ServiceResult<DateTime>.Ok(setContestStartDto.StartTime,
            $"{setContestStartDto.Key} start time updated.");
    }

    public async Task<ServiceResult<DateTime>> GetContestStartTimeAsync(string key) {
        var cc = await _configRepository.CheckExistence(key);
        if (cc == null) {
            return ServiceResult<DateTime>.Fail("کانفیگ مورد نظر یافت نشد!" , ErrorType.NotFound);
        }
        return ServiceResult<DateTime>.Ok(cc.StartTime);
    }
}