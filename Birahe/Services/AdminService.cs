using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Services;

public class AdminService {
    private readonly RiddleRepository _riddleRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationContext _context;
    private readonly UserRepository _userRepository;

    public AdminService(RiddleRepository riddleRepository,
        IMapper mapper,
        ApplicationContext context
        , UserRepository userRepository) {


        _riddleRepository = riddleRepository;
        _mapper = mapper;
        _context = context;
        _userRepository = userRepository;
    }


    // Riddle business
    public async Task<ServiceResult>AddRiddleAsync(AdminRiddleDto adminRiddleDto) {
        var riddleDtoUId = adminRiddleDto.Department + adminRiddleDto.No;

        Riddle riddle = _mapper.Map<Riddle>(adminRiddleDto);
        await _riddleRepository.AddRiddle(riddle);
        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("ثبت معما با خطا روبرو شد!" , ErrorType.ServerError);
        }
        return ServiceResult.Ok("معما با موفقیت ثبت شد!");
    }


    public async Task<ServiceResult<AdminRiddleDto>> EditRiddleAsync(AdminRiddleDto adminRiddleDto) {
        var riddleDtoUId = adminRiddleDto.Department + adminRiddleDto.No;

        var oldRiddle = _mapper.Map<Riddle>(adminRiddleDto);

        var flag = await _riddleRepository.EditRiddle(riddleDtoUId, oldRiddle);

        if (!flag) {
            return ServiceResult<AdminRiddleDto>.Fail("این معما قبلا ثبت نشده است.2", ErrorType.Validation);
        }

        var rows =await _context.SaveChangesAsync();
        if (rows==0) {
            return ServiceResult<AdminRiddleDto>.Fail("ویرایش معما با خطا رو به رو شد!", ErrorType.ServerError);
        }
        return ServiceResult<AdminRiddleDto>.Ok(adminRiddleDto , message:"معما با موفقیت ویرایش شد.");
    }

    public async Task<ServiceResult<List<AdminRiddleDto>>> GetAllRiddlesAsync() {
        var riddles =await _riddleRepository.GetRiddles();
        if (riddles.Count == 0) {
            return ServiceResult<List<AdminRiddleDto>>.NoContent("هیچ معمایی ثبت نشده است!");
        }

        var riddlesDto = _mapper.Map<List<AdminRiddleDto>>(riddles);

        return ServiceResult<List<AdminRiddleDto>>.Ok(riddlesDto, "خدمت شما.");
    }

    public async Task<ServiceResult> DeleteRiddleAsync(RemoveRiddleDto removeRiddleDto) {
        var uId = removeRiddleDto.Department + removeRiddleDto.No;


        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("حدف معما با خطا رو به رو شد!", ErrorType.ServerError);
        }

        return ServiceResult.Ok("معما با موفقیت حذف شد");
    }


    // User business

    public async Task<ServiceResult<List<AdminUserDto>>> GetAllUsersAsync() {
        var userList = await _userRepository.GetAllUser();
        if (userList == null) {
            return ServiceResult<List<AdminUserDto>>.NoContent("");
        }

        var userListDto = _mapper.Map<List<AdminUserDto>>(userList);

        return ServiceResult<List<AdminUserDto>>.Ok(userListDto, "خدمت شما");

    }


    public async Task<ServiceResult> BanUserAsync(BanUserDto banUserDto) {
        var flag = await _userRepository.BanUser(banUserDto);
        if (!flag) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است.");
        }

        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("فرآیند بن کردن با خطا رو به رو شد!", ErrorType.ServerError);
        }

        return ServiceResult.Ok("کاربر با موفقیت بن شد.");
    }

}