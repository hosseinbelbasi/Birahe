using Birahe.EndPoint.Constants.Enums;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.UserDto_s;
using Birahe.EndPoint.Models.ResultModels;
using Birahe.EndPoint.Repositories;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Birahe.EndPoint.Services;

public class UserService {
    private readonly UserRepository _userRepository;
    private readonly PaymentRepository _paymentRepository;
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly JwtService _jwtService;

    public UserService(UserRepository userRepository, ApplicationContext context, IMapper mapper,
        JwtService jwtService, PaymentRepository paymentRepository) {
        _userRepository = userRepository;
        _context = context;
        _mapper = mapper;
        _jwtService = jwtService;
        _paymentRepository = paymentRepository;
    }

    public async Task<ServiceResult<SignupResultDto>> SignupAsync(SignUpDto signUpDto) {
        if (signUpDto.Students.Count > 3)
            return ServiceResult<SignupResultDto>.Fail("یک تیم میتواند حداکثر 3 دانشجو داشته باشد!");

        if (signUpDto.Students.Count == 0)
            return ServiceResult<SignupResultDto>.Fail("حداقل یک دانشجو باید عضو تیم باشد!");

        User? exists = await _userRepository.CheckExistenceSignup(signUpDto.Username, signUpDto.TeamName);
        if (exists != null)
            return ServiceResult<SignupResultDto>.Fail(" نام کاربری یا نام تیم قبلاً استفاده شده است.");

        var duplicateStudentNos = signUpDto.Students
            .GroupBy(s => s.StudentNo)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateStudentNos.Any())
            return ServiceResult<SignupResultDto>.Fail("شماره دانشجویی تکراری در درخواست!");

        var studentNos = signUpDto.Students.Select(s => s.StudentNo).ToList();
        var anyExists = await _context.Students
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => studentNos
                .Contains(s.StudentNo));


        if (anyExists != null)
            return ServiceResult<SignupResultDto>.Fail($"{anyExists.StudentNo}" + " قبلا در سیستم ثبت شده است ");

        var user = _mapper.Map<User>(signUpDto);
        user.Students = _mapper.Map<List<Student>>(signUpDto.Students);

        await _userRepository.AddUser(user);
        var rows = await _context.SaveChangesAsync();

        if (rows <= 0)
            return ServiceResult<SignupResultDto>.Fail("ثبت نام با خطا مواجه شد!", ErrorType.ServerError);

        var result = new SignupResultDto() {
            Token = _jwtService.GenerateToken(user)
        };

        return ServiceResult<SignupResultDto>.Ok(result,
            "ثبت نام با موفقیت انجام شد! بعد از پرداخت مبلغ حساب کاربری شما فعال خواهد شد");
    }

    public async Task<ServiceResult<LoginResultDto>> LoginAsync(LoginDto loginDto) {
        var user = await _userRepository.Login(loginDto.Username, loginDto.Password);
        if (user == null) {
            return ServiceResult<LoginResultDto>.Fail("نام کاربری یا رمز عبور اشتباه است!", ErrorType.Validation);
        }

        if (user.Role == Role.PaymentPending) {
            return ServiceResult<LoginResultDto>.Fail("ابتدا هزینه ثبت نام را پرداخت کنید !", ErrorType.Forbidden);
        }

        if (user.IsBanned) {
            return ServiceResult<LoginResultDto>.Fail($" {"حساب کاربری شما مسدود شده است :"}  {user.BanReason}");
        }

        if (user.RemoveTime != null) {
            return ServiceResult<LoginResultDto>.Fail($"{"حساب کاربری یافت نشد ."}", ErrorType.NotFound);
        }

        var result = new LoginResultDto() {
            UserDto = _mapper.Map<UserDto>(user),
            Token = _jwtService.GenerateToken(user)
        };


        return ServiceResult<LoginResultDto>.Ok(result);
    }

    public async Task<ServiceResult> EditUsernameAsync(string oldUsername, EditUsernameDto editUsernameDto) {
        var user = await _userRepository.CheckExistence(oldUsername);
        if (user == null) {
            return ServiceResult.Fail(message: " کاربری پیدا نشد!", error: ErrorType.Validation);
        }

        var checkNewUsername = await _userRepository.CheckExistence(editUsernameDto.NewUsername);
        if (checkNewUsername != null) {
            return ServiceResult.Fail(message: " این نام کاربری در دسترس نیست!", error: ErrorType.Validation);
        }


        if (oldUsername == editUsernameDto.NewUsername) {
            return ServiceResult.Fail("نام کاربری قدیمی و جدید نمیتوانند یکسان باشند");
        }

        _userRepository.ChangeUsername(user, editUsernameDto.NewUsername);

        var rows = await _context.SaveChangesAsync();

        if (rows == 0) {
            ServiceResult.Fail(message: " تغییر نام کاربری با خطا مواجه شد!", error: ErrorType.ServerError);
        }

        return ServiceResult.Ok(message: "تغییر نام کاربری با موفقیت انجام شد!", success: true);
    }

    public async Task<ServiceResult> ChangePasswordAsync(string username, ChangePasswordDto changePasswordDto) {
        var user = await _userRepository.CheckExistence(username);
        if (user == null) {
            return ServiceResult.Fail("این کاربر قبلا ثبت نشده است!");
        }

        var flag = _userRepository.ChangePassword(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

        if (!flag) {
            return ServiceResult.Fail("نام کاربری یا رمز عبور صحیح نمی باشد!");
        }

        var rows = await _context.SaveChangesAsync();

        if (rows == 0) {
            ServiceResult.Fail(message: " تغییر رمز عبور با خطا مواجه شد!", error: ErrorType.ServerError);
        }

        return ServiceResult.Ok("تغییر رمز عبور با موفقیت انجام شد!");
    }

    public async Task<ServiceResult<string>> GetContestStartTimeAsync() {
        var config = await _context.ContestConfigs.FirstOrDefaultAsync(cc => cc.Key == "Contest");
        if (config == null) {
            return ServiceResult<string>.Fail("هنوز زمان شروع مسابقه تایین نشده است", ErrorType.NotFound);
        }

        var result = config.StartTime.ToString("yyyy-MM-ddTHH:mm:ssK");

        return ServiceResult<string>.Ok(result);
    }

    public async Task<ServiceResult<UserDto>> GetUserById(int userId) {
        var user = await _userRepository.FindUser(userId);
        if (user == null) {
            return ServiceResult<UserDto>.Fail("کاربر یافت نشد!", ErrorType.NotFound);
        }
        var userDto = _mapper.Map<UserDto>(user);
        return ServiceResult<UserDto>.Ok(userDto);
    }
}