using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Helpers.Extensions;
using Birahe.EndPoint.Models;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.UserDto_s;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services;
using Birahe.EndPoint.Validator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
    private readonly UserService _userService;


    public UsersController(UserService userService) {
        _userService = userService;
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Signup([FromBody] SignUpDto signUpDto) {
        var result = await _userService.SignupAsync(signUpDto);

        return this.MapServiceResult(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
        var result = await _userService.LoginAsync(loginDto);

        return this.MapServiceResult(result);
    }

    [HttpPatch("edit/username")]
    [Authorize]
    public async Task<IActionResult> EditUsername([FromBody] EditUsernameDto editUsernameDto) {
        var currentUsername = User.GetUsername();
        if (String.IsNullOrEmpty(currentUsername)) {
            return BadRequest();
        }

        var result = await _userService.EditUsernameAsync(currentUsername, editUsernameDto);
        return this.MapServiceResult(result);
    }

    [HttpPatch("edit/password")]
    [Authorize]
    public async Task<IActionResult> EditPassword([FromBody] ChangePasswordDto changePasswordDto) {
        var currentUsername = User.GetUsername();
        if (String.IsNullOrEmpty(currentUsername)) {
            return BadRequest();
        }

        var result = await _userService.ChangePasswordAsync(currentUsername, changePasswordDto);
        return this.MapServiceResult(result);
    }

    [HttpGet("contest/start")]
    public async Task<IActionResult> GetContestStartTime() {
        var result = await _userService.GetContestStartTimeAsync();
        return this.MapServiceResult(result);
    }
}