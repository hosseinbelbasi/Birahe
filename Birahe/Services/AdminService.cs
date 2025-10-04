using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Birahe.EndPoint.Services;

public class AdminService {
    private readonly RiddleRepository _riddleRepository;
    private readonly IMapper _mapper;
    private readonly ApplicationContext _context;

    public AdminService(RiddleRepository riddleRepository, IMapper mapper, ApplicationContext context) {
        _riddleRepository = riddleRepository;
        _mapper = mapper;
        _context = context;
    }


    // Riddle business
    public async Task<ServiceResult>AddRiddleAsync(RiddleDto riddleDto) {
        var riddleDtoUId = riddleDto.Department + riddleDto.No;
        var exists = await _riddleRepository.CheckExistence(riddleDtoUId);
        if (exists != null) {
            return ServiceResult.Fail("این معما قبلا ثبت شده است.");
        }

        Riddle riddle = _mapper.Map<Riddle>(riddleDto);
        await _riddleRepository.AddRiddle(riddle);
        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("ثبت معما با خطا روبرو شد!" , ErrorType.ServerError);
        }
        return ServiceResult.Ok("معما با موفقیت ثبت شد!");
    }


    public async Task<ServiceResult<RiddleDto>> EditRiddleAsync(RiddleDto riddleDto) {
        var riddleDtoUId = riddleDto.Department + riddleDto.No;
        var exists = await _riddleRepository.CheckExistence(riddleDtoUId);
        if (exists == null) {
            return ServiceResult<RiddleDto>.Fail("این معما قبلا ثبت نشده است.");
        }

        var oldRiddle = _mapper.Map<Riddle>(riddleDto);

        var flag = await _riddleRepository.EditRiddle(riddleDtoUId, oldRiddle);

        if (!flag) {
            return ServiceResult<RiddleDto>.Fail("این معما قبلا ثبت نشده است.2", ErrorType.Validation);
        }

        var rows =await _context.SaveChangesAsync();
        if (rows==0) {
            return ServiceResult<RiddleDto>.Fail("ویرایش معما با خطا رو به رو شد!", ErrorType.ServerError);
        }
        return ServiceResult<RiddleDto>.Ok(riddleDto , message:"معما با موفقیت ویرایش شد.");
    }

    public async Task<ServiceResult<List<RiddleDto>>> GetRiddlesAsync() {
        var riddles =await _riddleRepository.GetRiddles();
        if (riddles.Count == 0) {
            return ServiceResult<List<RiddleDto>>.NoContent("هیچ معمایی ثبت نشده است!");
        }

        var riddlesDto = _mapper.Map<List<RiddleDto>>(riddles);

        return ServiceResult<List<RiddleDto>>.Ok(riddlesDto, "خدمت شما.");
    }

    public async Task<ServiceResult> DeleteRiddleAsync(RemoveRiddleDto removeRiddleDto) {
        var uId = removeRiddleDto.Department + removeRiddleDto.No;
        var toDeleteRiddle = await _riddleRepository.CheckExistence(uId);
        if (toDeleteRiddle == null) {
            return ServiceResult.Fail("این معما قبلا ثبت نشده است.");
        }

        var flag = await _riddleRepository.RemoveRiddle(uId);
        if (!flag) {
            return ServiceResult.Fail("این معما قبلا ثبت نشده است.");
        }

        var rows = await _context.SaveChangesAsync();
        if (rows == 0) {
            return ServiceResult.Fail("حدف معما با خطا رو به رو شد!", ErrorType.ServerError);
        }

        return ServiceResult.Ok("معما با موفقیت حذف شد");
    }

}