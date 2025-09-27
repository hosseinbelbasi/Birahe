using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Birahe.EndPoint.Controllers;

[Route("User/[action]")]
public class UserController : Controller {
    private ApplicationContext _context;
    private UserRepository _userRepository;
    private IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly JwtService _jwtService;


    public UserController(ApplicationContext context, UserRepository userRepository, IMapper mapper, IConfiguration config, JwtService jwtService) {
        _context = context;
        _userRepository = userRepository;
        _mapper = mapper;
        _config = config;
        _jwtService = jwtService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto) {
        if (!ModelState.IsValid) {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new {errors});
        }

        if (signUpDto.Students.Count > 3) {
            return BadRequest("یک تیم میتواند حداکثر 3 دانشجو داشته باشد!");
        }

        if (signUpDto.Students.Count == 0) {
            return Ok(new { error = "حئاقل یک دانشجو باید عضو تیم باشد!"});
        }

        User? exsits = await _userRepository.CheckExistence(signUpDto.Username);
        if (exsits!=null) {
            return BadRequest(new { message = "این نام کاربری قبلاً استفاده شده است." });
        }

        var duplicateStudentNos = signUpDto.Students
            .GroupBy(s => s.StudentNo)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateStudentNos.Any()) {
            return BadRequest(new {message ="شماره دانشجویی تکراری در درخواست!" , dulicates = duplicateStudentNos} );
        }

        var studentNos = signUpDto.Students.Select(s => s.StudentNo).ToList();

        bool anyExists = await _context.Students
            .AnyAsync(s => studentNos.Contains(s.StudentNo));

        if (anyExists)
        {
            return BadRequest("هر دانشجو میتواند حداکثر عضو یک تیم باشد!");
        }


        var user = _mapper.Map<User>(signUpDto);
        user.Students = _mapper.Map<List<Student>>(signUpDto.Students);

        var userName =  "team"+Guid.NewGuid().ToString("N").Substring(0, 10);
        user.Username = userName;

        await _userRepository.AddUser(user);
        var count = await _context.SaveChangesAsync();

        if (count <= 0) {
            return StatusCode(500, new { message = "ثبت نام با خطا مواجه شد!" });
        }

        return Ok(new {
            message = "ثبت نام با موفقیت انجام شد!" ,
            username = userName,
        });

    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LogIn([FromBody] LoginDto loginDto) {
        if (!ModelState.IsValid) {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new {errors});
        }
        var user =await _userRepository.Login(loginDto.UserName, loginDto.Password);
        if (user == null) {
            return Ok(new { meassage = "نام کاربری یا رمز عبور اشتباه است!" });
        }

        UserDto userDto = _mapper.Map<UserDto>(user);


        var tokenString = _jwtService.GenerateToken(user);

        return Ok(new {
            message = "خوش آمدید!",
            team = userDto,
            token = tokenString
        });

    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> EditUserName([FromBody] EditUsernameDto editUsernameDto){
        if (!ModelState.IsValid) {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new {errors});
        }

        if (editUsernameDto.OldUsername == editUsernameDto.NewUsername) {
            return BadRequest(new {message = "same"});
        }

        var flag = await _userRepository.ChangeUsername(editUsernameDto.OldUsername, editUsernameDto.NewUsername);
        if (!flag) {
            return BadRequest(new {message = "repofailed"});
        }

        await _context.SaveChangesAsync();

        return Ok(new{message = "نام کاربری با موفقیت تعویض شد!"});


    }




}