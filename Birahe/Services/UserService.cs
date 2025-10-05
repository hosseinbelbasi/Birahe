using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public UserService(UserRepository userRepository, ApplicationContext context, IMapper mapper)
    {
        _userRepository = userRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceResult<User>> SignupAsync(SignUpDto signUpDto)
    {
        if (signUpDto.Students.Count > 3)
            return ServiceResult<User>.Fail("یک تیم میتواند حداکثر 3 دانشجو داشته باشد!");

        if (signUpDto.Students.Count == 0)
            return ServiceResult<User>.Fail("حداقل یک دانشجو باید عضو تیم باشد!");

        User? exists = await _userRepository.CheckExistence(signUpDto.Username);
        if (exists != null)
            return ServiceResult<User>.Fail("این نام کاربری قبلاً استفاده شده است.");

        var duplicateStudentNos = signUpDto.Students
            .GroupBy(s => s.StudentNo)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateStudentNos.Any())
            return ServiceResult<User>.Fail("شماره دانشجویی تکراری در درخواست!");

        var studentNos = signUpDto.Students.Select(s => s.StudentNo).ToList();
        bool anyExists = await _context.Students
            .AnyAsync(s => studentNos.Contains(s.StudentNo));

        if (anyExists)
            return ServiceResult<User>.Fail("هر دانشجو میتواند حداکثر عضو یک تیم باشد!");

        var user = _mapper.Map<User>(signUpDto);
        user.Students = _mapper.Map<List<Student>>(signUpDto.Students);

        await _userRepository.AddUser(user);
        var count = await _context.SaveChangesAsync();

        if (count <= 0)
            return ServiceResult<User>.Fail("ثبت نام با خطا مواجه شد!", ErrorType.ServerError);

        return ServiceResult<User>.Ok(user, "ثبت نام با موفقیت انجام شد!");
    }

    public async Task<ServiceResult<User>> LoginAsync(LoginDto loginDto) {
        var user =await _userRepository.Login(loginDto.Username, loginDto.Password);
        if (user == null) {
            return ServiceResult<User>.Ok(user,success:false,message:"نام کاربری یا رمز عبور اشتباه است!");
        }

        if (user.IsBanned) {
            return ServiceResult<User>.Fail("حساب کاربری شما مسدود شده است.");
        }

        return ServiceResult<User>.Ok(user);
    }

    public async Task<ServiceResult<String>> EditUsernameAsync(EditUsernameDto editUsernameDto) {
        var flag = await _userRepository.ChangeUsername(editUsernameDto.OldUsername, editUsernameDto.NewUsername);
        if (!flag) {
            ServiceResult<string>.Fail(message: " کاربری پیدا نشد!", error: ErrorType.Validation);
        }

        var rows =  await _context.SaveChangesAsync();

        if (rows == 0) {
            ServiceResult<string>.Fail(message: " تغییر نام کاربری با خطا مواجه شد!", error: ErrorType.ServerError);
        }

        return ServiceResult<string>.Ok(message: "تغییر نام کاربری با موفقیت انجام شد!", success:true, data:editUsernameDto.NewUsername);
    }

    public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto) {
        var flag = await _userRepository.ChangePassword(changePasswordDto);
        if (!flag) {
            return ServiceResult.Fail("نام کاربری یا رمز عبور صحیح نمی باشد!");
        }
        var rows =  await _context.SaveChangesAsync();

        if (rows == 0) {
            ServiceResult.Fail(message: " تغییر رمز عبور با خطا مواجه شد!", error: ErrorType.ServerError);
        }

        return ServiceResult.Ok("تغییر رمز عبور با موفقیت انجام شد!");
    }


}
