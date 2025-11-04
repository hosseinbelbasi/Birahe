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
    private readonly MediaService _mediaService;
    private readonly ContestConfigRepository _configRepository;
    private readonly MemoryCacheService _cacheService;

    public AdminService(
        RiddleRepository riddleRepository,
        IMapper mapper,
        ApplicationContext context,
        UserRepository userRepository,
        MediaService mediaService, ContestConfigRepository configRepository, MemoryCacheService cacheService) {
        _riddleRepository = riddleRepository;
        _mapper = mapper;
        _context = context;
        _userRepository = userRepository;
        _mediaService = mediaService;
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
        UploadRiddleFilesAsync(int riddleId, IFormFile? hintFile, IFormFile? rewardFile) {
        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);
        if (riddle == null) {
            return ServiceResult.Fail("معما یافت نشد!", ErrorType.NotFound);
        }

        if (hintFile == null && rewardFile == null) {
            return ServiceResult.Fail("هیچ عکسی آپلود نکرده اید!");
        }

        if (hintFile != null) {
            var result = await _mediaService.SaveFileAsync(hintFile);
            if (!result.Success) {
                return ServiceResult.Fail(result.Message, result.Error);
            }

            var (fileName, mediaType) = result.Data;

            riddle.HintMediaType = mediaType;
            riddle.HintFileName = fileName;
        }

        if (rewardFile != null) {
            var result = await _mediaService.SaveFileAsync(rewardFile);
            if (!result.Success) {
                return ServiceResult.Fail(result.Message, result.Error);
            }

            var (fileName, mediaType) = result.Data;

            riddle.RewardMediaType = mediaType;
            riddle.RewardFileName = fileName;
        }

        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("دخیره عکس با خطا مواجه شد!", ErrorType.ServerError);
        }

        _cacheService.Remove(CacheKeys.AdminAllRiddles);

        return ServiceResult.Ok("عکس(ها) با موفقیت ذخیره شدند!");
    }


    public async Task<ServiceResult<(byte[] File, string ContentType)>> GetRiddleMediaByIdAsync(int riddleId,
        string type) {
        var riddle = await _context.Riddles.FindAsync(riddleId);
        if (riddle == null)
            return ServiceResult<(byte[], string)>.Fail("معما پیدا نشد!", ErrorType.NotFound);

        string? fileName = type.ToLower() switch {
            "hint" => riddle.HintFileName,
            "reward" => riddle.RewardFileName,
            _ => null
        };

        if (string.IsNullOrEmpty(fileName))
            return ServiceResult<(byte[], string)>.Fail("فایل یافت نشد!", ErrorType.NotFound);

        var result = await _mediaService.ReadFileAsync(fileName);
        if (!result.Success)
            return ServiceResult<(byte[], string)>.Fail(result.Message ?? "خطا در خواندن فایل!",
                result.Error);

        return ServiceResult<(byte[], string)>.Ok(result.Data);
    }


    // =======================User business===================

    public async Task<ServiceResult<List<AdminUserDto>>> GetAllUsersAsync() {
        var userList = await _cacheService.GetOrSetAsync(
            CacheKeys.AdminAllUsers,
            async () => await _userRepository.GetAllUser(),
            TimeSpan.FromMinutes(10)
        );
        if (userList == null) {
            return ServiceResult<List<AdminUserDto>>.NoContent();
        }

        var userListDto = _mapper.Map<List<AdminUserDto>>(userList);

        return ServiceResult<List<AdminUserDto>>.Ok(userListDto, "خدمت شما");
    }


    public async Task<ServiceResult> BanUserAsync(int userId, BanUserDto banUserDto) {
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


    public async Task<ServiceResult> EditUserUsernameAsync(int userId, AdminEditUserUsernameDto usernameDto) {
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

    public async Task<ServiceResult> SetContestConfigAsync(SetContestConfigDto setContestConfigDto) {
        var exists = await _configRepository.CheckExistence(setContestConfigDto.Key);
        if (exists == null) {
            var config = _mapper.Map<ContestConfig>(setContestConfigDto);
            await _configRepository.AddContestConfig(config);
        }
        else {
            exists.StartTime = setContestConfigDto.StartTime;
            exists.EndTime = setContestConfigDto.EndTime;
            _configRepository.UpdateContestConfig(exists);
        }

        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("خطا در ثبت عملیات", ErrorType.ServerError);
        }

        return ServiceResult.Ok($"{setContestConfigDto.Key} با موفقیت ثبت شد.");
    }

    public async Task<ServiceResult<ContestConfigDto>> GetContestConfigByKeyAsync(string key) {
        var cc = await _configRepository.CheckExistence(key);
        if (cc == null) {
            return ServiceResult<ContestConfigDto>.Fail("کانفیگ مورد نظر یافت نشد!", ErrorType.NotFound);
        }

        return ServiceResult<ContestConfigDto>.Ok(_mapper.Map<ContestConfigDto>(cc));
    }


    public async Task<ServiceResult<List<ContestConfigDto>>> GetAllContestConfigsAsync() {
        var configs = await _configRepository.GetAllConfigs();
        if (configs.Count ==0) {
            return ServiceResult<List<ContestConfigDto>>.NoContent();
        }

        var configsDto = _mapper.Map<List<ContestConfigDto>>(configs);

        return ServiceResult<List<ContestConfigDto>>.Ok(configsDto);
    }
}