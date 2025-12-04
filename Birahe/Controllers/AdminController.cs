using Birahe.EndPoint.Helpers.Extensions;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
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
    public async Task<IActionResult> EditRiddle(int riddleId, [FromBody] AddRiddleDto addRiddleDto) {
        var result = await _adminService.EditRiddleAsync(riddleId, addRiddleDto);
        return this.MapServiceResult(result);
    }

    [HttpDelete("riddles/{riddleId:int}")]
    public async Task<IActionResult> RemoveRiddle(int riddleId) {
        var result = await _adminService.DeleteRiddleAsync(riddleId);

        return this.MapServiceResult(result);
    }

    [HttpGet("riddles/{riddleId:int}")]
    public async Task<IActionResult> GetRiddleById(int riddleId) {
        var result = await _adminService.GetRiddleByIdAsync(riddleId);
        return this.MapServiceResult(result);
    }

    [HttpPost("riddles/{riddleId:int}/media")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadRiddleImages(int riddleId, IFormFile? hintFile, IFormFile? rewardFile) {
        var result = await _adminService.UploadRiddleFilesAsync(riddleId, hintFile, rewardFile);
        return this.MapServiceResult(result);
    }

    [HttpGet("riddles/{riddleId:int}/media/{type}")]
    public async Task<IActionResult> GetRiddleImage(int riddleId, string type) {
        var result = await _adminService.GetRiddleMediaByIdAsync(riddleId, type);
        return this.MapMediaServiceResult(result);
    }


    //==============User Actions===============

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers() {
        var result = await _adminService.GetAllUsersAsync();
        return this.MapServiceResult(result);
    }

    [HttpPatch("users/{userId:int}/ban")]
    public async Task<IActionResult> BanUser(int userId, [FromBody] BanUserDto banUserDto) {
        var result = await _adminService.BanUserAsync(userId, banUserDto);
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
    public async Task<IActionResult> EditUserPassword(int userId, [FromBody] AdminEditUserPasswordDto userPasswordDto) {
        var result = await _adminService.EditUserPasswordAsync(userId, userPasswordDto);
        return this.MapServiceResult(result);
    }


    [HttpGet("users/{userId:int}/status")]
    public async Task<IActionResult> GetUserStatusById(int userId) {
        var result = await _adminService.GetUserStatusAsync(userId);
        return this.MapServiceResult(result);
    }

    [HttpGet("users/leaderboard")]
    public async Task<IActionResult> GetLeaderBoard() {
        var result = await _adminService.GetLeaderBoardAsync();
        return this.MapServiceResult(result);
    }


    // ================= Contest Configs Actins =====================

    [HttpPost("contest/config")]
    public async Task<IActionResult> SetContestStartTime([FromBody] SetContestConfigDto setContestConfigDto) {
        var result = await _adminService.SetContestConfigAsync(setContestConfigDto);
        return this.MapServiceResult(result);
    }

    [HttpGet("contest/config/{key}")]
    public async Task<IActionResult> GetContestStartTime(string key) {
        var result = await _adminService.GetContestConfigByKeyAsync(key);
        return this.MapServiceResult(result);
    }

    [HttpGet("contest/config")]
    public async Task<IActionResult> GetAllContestConfigs() {
        var result = await _adminService.GetAllContestConfigsAsync();
        return this.MapServiceResult(result);
    }
}