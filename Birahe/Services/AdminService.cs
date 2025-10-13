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

    public AdminService(
        RiddleRepository riddleRepository,
        IMapper mapper,
        ApplicationContext context,
        UserRepository userRepository,
        ImageService imageService) {


        _riddleRepository = riddleRepository;
        _mapper = mapper;
        _context = context;
        _userRepository = userRepository;
        _imageService = imageService;
    }


    // ================Riddle business=============
    public async Task<ServiceResult>AddRiddleAsync(AddRiddleDto addRiddleDto) {
        var riddleDtoUId = addRiddleDto.Department + addRiddleDto.No;
        var exists = await _riddleRepository.CheckExistence(riddleDtoUId);
        if (exists != null) {
            return ServiceResult.Fail("این معما قبلا ثبت شده است!");
        }

        Riddle riddle = _mapper.Map<Riddle>(addRiddleDto);
        await _riddleRepository.AddRiddle(riddle);
        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("ثبت معما با خطا روبرو شد!" , ErrorType.ServerError);
        }
        return ServiceResult.Ok("معما با موفقیت ثبت شد!");
    }

    public async Task<ServiceResult<AdminRiddleDto>> EditRiddleAsync(AdminRiddleDto adminRiddleDto) {

        var exists = await _riddleRepository.FindRiddleAsync(adminRiddleDto.Id);
        if (exists == null) {
            return ServiceResult<AdminRiddleDto>.Fail("این معما قبلا ثبت نشده است.");

        }


        var oldRiddle = _mapper.Map<Riddle>(adminRiddleDto);

        _riddleRepository.EditRiddle(exists, oldRiddle);



        var rows =await _context.SaveChangesAsync();
        if (rows==0) {
            return ServiceResult<AdminRiddleDto>.Fail("ویرایش معما با خطا رو به رو شد!", ErrorType.ServerError);
        }
        return ServiceResult<AdminRiddleDto>.Ok(adminRiddleDto , message:"معما با موفقیت ویرایش شد.");
    }

    public async Task<ServiceResult<List<AdminRiddleDto>>> GetAllRiddlesAsync() {
        var riddles =await _riddleRepository.GetRiddles();
        if (riddles.Count == 0) {
            return ServiceResult<List<AdminRiddleDto>>.NoContent();
        }

        var riddlesDto = _mapper.Map<List<AdminRiddleDto>>(riddles);

        return ServiceResult<List<AdminRiddleDto>>.Ok(riddlesDto, "خدمت شما.");
    }

    public async Task<ServiceResult> DeleteRiddleAsync(RemoveRiddleDto removeRiddleDto) {
        var uId = removeRiddleDto.Department + removeRiddleDto.No;
        var riddle = await _riddleRepository.CheckExistence(uId);
        if (riddle == null) {
            return ServiceResult.Fail("این معما قبلا ثبت نشده است.");
        }

        _riddleRepository.RemoveRiddle(riddle);

        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("حدف معما با خطا رو به رو شد!", ErrorType.ServerError);
        }

        return ServiceResult.Ok("معما با موفقیت حذف شد");
    }

    public async Task<ServiceResult<AdminRiddleDto>> GetRiddleByIdAsync(int id) {
        var riddle =await _riddleRepository.FindRiddleAsync(id);
        if (riddle == null) {
            return ServiceResult<AdminRiddleDto>.Fail("این معما قبلا ثبت نشده است.");
        }

        var riddleDto = _mapper.Map<AdminRiddleDto>(riddle);
        return ServiceResult<AdminRiddleDto>.Ok(riddleDto);
    }

    public async Task<ServiceResult>
        UploadRiddleImageAsync(int riddleId, IFormFile? hintImage, IFormFile? rewardImage)
    {
        var riddle = await _riddleRepository.FindRiddleAsync(riddleId);
        if (riddle == null) {
            return ServiceResult.Fail("معما یافت نشد!", ErrorType.NotFound);
        }

        if (hintImage== null && rewardImage == null) {
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

        var rows =await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("دخیره عکس با خطا مواجه شد!", ErrorType.ServerError);
        }

        return ServiceResult.Ok("عکس(ها) با موفقیت ذخیره شدند!");
    }


    public async Task<ServiceResult<List<Object>>> GetRiddleImagesByIdAsync(int riddleId) {
        var riddle = await _context.Riddles.FindAsync(riddleId);
        if (riddle == null)
            return ServiceResult<List<object>>.Fail("معما پیدا نشد!", ErrorType.NotFound);

        // Build physical paths (assuming images are stored under wwwroot/images/riddles/)
        var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var hintImagePath = string.IsNullOrEmpty(riddle.HintImageFileName) ? null : Path.Combine(wwwRootPath, riddle.HintImageFileName.TrimStart('/'));
        var rewardImagePath = string.IsNullOrEmpty(riddle.RewardImageFileName) ? null : Path.Combine(wwwRootPath, riddle.RewardImageFileName.TrimStart('/'));

        // Prepare result list
        var imageFiles = new List<object>();

        if (hintImagePath != null && System.IO.File.Exists(hintImagePath))
        {
            var hintBytes = await System.IO.File.ReadAllBytesAsync(hintImagePath);
            imageFiles.Add(new {
                type = "hintImage",
                fileName = Path.GetFileName(hintImagePath),
                contentType = GetContentType(hintImagePath),
                file = Convert.ToBase64String(hintBytes)
            });
        }

        if (rewardImagePath != null && System.IO.File.Exists(rewardImagePath))
        {
            var rewardBytes = await System.IO.File.ReadAllBytesAsync(rewardImagePath);
            imageFiles.Add(new {
                type = "rewardImage",
                fileName = Path.GetFileName(rewardImagePath),
                contentType = GetContentType(rewardImagePath),
                file = Convert.ToBase64String(rewardBytes)
            });
        }

        if (imageFiles.Count == 0)
            return ServiceResult<List<object>>.NoContent();

        return ServiceResult<List<object>>.Ok(imageFiles);
    }

    private static string GetContentType(string path)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }


    // =======================User business===================

    public async Task<ServiceResult<List<AdminUserDto>>> GetAllUsersAsync() {
        var userList = await _userRepository.GetAllUser();
        if (userList == null) {
            return ServiceResult<List<AdminUserDto>>.NoContent();
        }

        var userListDto = _mapper.Map<List<AdminUserDto>>(userList);

        return ServiceResult<List<AdminUserDto>>.Ok(userListDto, "خدمت شما");

    }


    public async Task<ServiceResult> BanUserAsync(BanUserDto banUserDto) {
        var user =await _userRepository.CheckExistence(banUserDto.Username);
        if (user==null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }
        _userRepository.BanUser(user, banUserDto.BsnReason);


        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("فرآیند بن کردن با خطا رو به رو شد!", ErrorType.ServerError);
        }

        return ServiceResult.Ok("کاربر با موفقیت بن شد.");
    }

    public async Task<ServiceResult> UnBanUserAsync(int userId) {
        var user =await _userRepository.FindBannedUser(userId);
        if (user==null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }
        _userRepository.UnBanUser(user);


        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("فرآیند رفع مسدود کردن با خطا رو به رو شد!", ErrorType.ServerError);
        }

        return ServiceResult.Ok("کاربر با موفقیت رفع مسدود شد.");
    }


    public async Task<ServiceResult> EditUserUsernameAsync(AdminEditUserUsernameDto usernameDto) {
        var user =await _userRepository.CheckExistence(usernameDto.OldUsername);
        if (user==null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }

        _userRepository.ChangeUsername(user, usernameDto.NewUsername);

        var rows = await _context.SaveChangesAsync();

        if (rows == 0) {
            return ServiceResult.Fail("تغییر نام کاربری با خطا مواجه شد!");
        }
        return ServiceResult.Ok("نام کاربری با موفقیت تغییر یافت.");
    }

    public async Task<ServiceResult> EditUserPasswordAsync(AdminEditUserPasswordDto userPasswordDto) {
        var user =await _userRepository.CheckExistence(userPasswordDto.Username);
        if (user==null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }


        _userRepository.AdminChangePassword(user,userPasswordDto.NewPassword);

        var rows = await _context.SaveChangesAsync();

        if (rows == 0) {
            return ServiceResult.Fail("تغییر رمز عبور با خطا مواجه شد!");
        }
        return ServiceResult.Ok("رمز عبور با موفقیت تغییر یافت.");
    }

    public async Task<ServiceResult<List<AdminContestItemDto>?>> GetUserStatusAsync(int userId) {
        var list = await _userRepository.AdminGetUserStausAsync(userId);

        if (list == null || list.Count == 0)
            return ServiceResult<List<AdminContestItemDto>?>.Fail( "برای این کاربر هیچ سابقه‌ای یافت نشد." , ErrorType.NotFound);

        var listDto = _mapper.Map<List<AdminContestItemDto>>(list);



        return ServiceResult<List<AdminContestItemDto>?>.Ok(listDto);
    }

}