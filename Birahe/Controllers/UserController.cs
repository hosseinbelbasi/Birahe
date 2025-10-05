using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services;
using Birahe.EndPoint.Validator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;

[ApiController]
[Route("User/[action]")]
public class UserController : Controller {
    private readonly ApplicationContext _context;
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly JwtService _jwtService;
    private readonly UserService _userService;


    public UserController(ApplicationContext context, UserRepository userRepository, IMapper mapper,  JwtService jwtService, UserService userService) {
        _context = context;
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtService = jwtService;
        _userService = userService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto) {

        var result = await _userService.SignupAsync(signUpDto);

        if (!result.Success)
        {
            return result.Error switch
            {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound   => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.NoContent=> NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        var userDto = _mapper.Map<UserDto>(result.Data!);
        var jwtToken = _jwtService.GenerateToken(result.Data!);

        return Ok(new {
            message = result.Message,
            user = userDto,
            token = jwtToken
        });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LogIn([FromBody] LoginDto loginDto) {

        var result =await _userService.LoginAsync(loginDto);

        if (!result.Success)
        {
            return result.Error switch
            {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound   => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }


        var tokenString = _jwtService.GenerateToken(result.Data!);
        var userDto = _mapper.Map<UserDto>(result.Data!);

        return Ok(new {
            message = "خوش آمدید!",
            team = userDto,
            token = tokenString
        });

    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EditUserName([FromBody] EditUsernameDto editUsernameDto){

        var result =await _userService.EditUsernameAsync(editUsernameDto);

        if (!result.Success)
        {
            return result.Error switch
            {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound   => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new{message = result.Message});


    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EditPassword([FromBody] ChangePasswordDto changePasswordDto) {
        var result =await _userService.ChangePasswordAsync(changePasswordDto);

        if (!result.Success)
        {
            return result.Error switch
            {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound   => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new{message = result.Message});
    }

}