using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Helpers.Extensions;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class AdminController : Controller {
    private readonly AdminService _adminService;


    public AdminController(AdminService adminService) {
        _adminService = adminService;
    }

    [HttpPost("riddles")]
    public async Task<IActionResult> AddRiddle([FromBody] AddRiddleDto addRiddleDto) {
        var result = await _adminService.AddRiddleAsync(addRiddleDto);
        return this.MapServiceResult(result);
    }


    [HttpGet("riddles")]
    public async Task<IActionResult> GetAllRiddles() {
        var result = await _adminService.GetAllRiddlesAsync();
        return this.MapServiceResult(result);
    }

    [HttpPut("riddles/{riddleId:int}")]
    public async Task<IActionResult> EditRiddle(int riddleId,[FromBody] AdminRiddleDto adminRiddleDto) {
        var result = await _adminService.EditRiddleAsync(riddleId,adminRiddleDto);
        return this.MapServiceResult(result);
    }

    [HttpDelete("riddles/{riddleId:int}")]
    public async Task<IActionResult> RemoveRiddle(int riddleId) {
        var result = await _adminService.DeleteRiddleAsync(riddleId);

        return this.MapServiceResult(result);
    }

    [HttpGet("riddles/{riddleId:int}")]
    public async Task<IActionResult> GetRiddleById(int id) {
        var result = await _adminService.GetRiddleByIdAsync(id);
        return this.MapServiceResult(result);
    }

    [HttpPost("riddles/{riddleId:int}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadRiddleImages(int riddleId, IFormFile? hintImage, IFormFile? rewardImage) {
        var result = await _adminService.UploadRiddleImageAsync(riddleId, hintImage, rewardImage);
        return this.MapServiceResult(result);
    }

    [HttpGet("riddles/{riddleId:int}/images/{type}")]
    public async Task<IActionResult> GetRiddleImage(int riddleId, string type) {
        var result = await _adminService.GetRiddleImageByIdAsync(riddleId, type);
        return this.MapImageServiceResult(result);
    }


    //==============User Actions===============

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers() {
        var result = await _adminService.GetAllUsersAsync();
        return this.MapServiceResult(result);
    }

    [HttpPatch("users/{userId:int}/ban")]
    public async Task<IActionResult> BanUser(int userId, [FromBody] BanUserDto banUserDto) {
        var result = await _adminService.BanUserAsync(userId,banUserDto);
        return this.MapServiceResult(result);
    }

    [HttpPatch("users/{userId:int}/unban")]
    public async Task<IActionResult> UnBanUser(int userId) {
        var result = await _adminService.UnBanUserAsync(userId);
        return this.MapServiceResult(result);
    }

    [HttpPatch("users/{userId:int}/username")]
    public async Task<IActionResult> EditUserUsername(int userId, [FromBody] AdminEditUserUsernameDto usernameDto) {
        var result = await _adminService.EditUserUsernameAsync(userId, usernameDto);
        return this.MapServiceResult(result);
    }

    [HttpPatch("users/{userId:int}/password")]
    public async Task<IActionResult> EditUserPassword(int userId,[FromBody] AdminEditUserPasswordDto userPasswordDto) {
        var result = await _adminService.EditUserPasswordAsync(userId, userPasswordDto);
        return this.MapServiceResult(result);
    }


    [HttpGet("users/{userId:int}/status")]
    public async Task<IActionResult> GetUserStatusById(int userId) {
        var result = await _adminService.GetUserStatusAsync(userId);
        return this.MapServiceResult(result);
    }


    // ================= Contest Configs Actins =====================

    [HttpPost("contest/config")]
    public async Task<IActionResult> SetContestStartTime([FromBody] SetContestStartDto setContestStartDto) {
        var result = await _adminService.SetContestStartTimeAsync(setContestStartDto);
        return this.MapServiceResult(result);
    }

    [HttpGet("contest/config/{key}")]
    public async Task<IActionResult> GetContestStartTime(string key) {
        var result = await _adminService.GetContestStartTimeAsync(key);
        return this.MapServiceResult(result);
    }


}