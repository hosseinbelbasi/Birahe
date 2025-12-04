using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Birahe.EndPoint.ControllerAttributes;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Helpers.Extensions;
using Birahe.EndPoint.Models.Dto.ContestDto_s;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Birahe.EndPoint.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "user, admin")]
[ContestTimeAuthorize("Contest")]
public class ContestController : Controller {
    private readonly ContestService _contestService;


    public ContestController(ContestService contestService) {
        _contestService = contestService;
    }


    [HttpGet("riddles")]
    public async Task<IActionResult> GetAllRiddles() {

        var userId = User.GetUserId();
        if (userId == -1) {
            return BadRequest();
        }

        var result = await _contestService.GetAllRiddlesWithStatusAsync(userId);

        return this.MapServiceResult(result);
    }

    [HttpGet("riddles/{id:int}/open")]
    public async Task<IActionResult> OpenRiddle(int id) {
        var userId = User.GetUserId();
        if (userId == -1) {
            return BadRequest();
        }

        var result = await _contestService.OpenRiddleAsync(userId, id);
        return this.MapServiceResult(result);
    }

    [HttpGet("riddles/{id:int}/hint")]
    public async Task<IActionResult> OpenHint(int id) {
        var userId = User.GetUserId();
        if (userId == -1) {
            return BadRequest();
        }

        var result = await _contestService.OpenRiddleHintAsync(userId, id);
        return this.MapServiceResult(result);
    }

    [HttpPost("riddles/{id:int}")]
    public async Task<IActionResult> SubmitAnswer(int id, [FromBody] SubmitAnswerDto submitAnswerDto) {
        var userId = User.GetUserId();
        if (userId == -1) {
            return BadRequest();
        }

        var result = await _contestService.SubmitAnswerAsync(userId, id, submitAnswerDto);
        return this.MapServiceResult(result);
    }

    [HttpGet("riddles/{id:int}")]
    public async Task<IActionResult> GetRiddle(int id) {
        var userId = User.GetUserId();
        if (userId == -1) {
            return BadRequest();
        }

        var result = await _contestService.GetRiddleWithStatusAsync(userId, id);
        return this.MapServiceResult(result);
    }

    // [HttpGet("leaderboard")]
    // public async Task<IActionResult> GetLeaderBoard() {
    //     var result = await _contestService.GetLeaderBoardAsync();
    //     return this.MapServiceResult(result);
    // }

    [HttpGet("user/me/balance")]
    public async Task<IActionResult> GetUserBalance() {
        var userId = User.GetUserId();
        if (userId == -1) {
            return NotFound();
        }

        var result = await _contestService.GetUserBalanceAsync(userId);

        return this.MapServiceResult(result);
    }

    // =================== file Retrieval ========================

    [HttpGet("riddles/{id:int}/media/reward")]
    public async Task<IActionResult> GetRewardMedia(int id) {
        var userId = User.GetUserId();
        if (userId == -1)
            return NotFound();

        var result = await _contestService.GetRewardMediaAsync(userId, id);

        return this.MapMediaServiceResult(result);
    }

    [HttpGet("riddles/{id:int}/media/hint")]
    public async Task<IActionResult> GetHintMedia(int id) {
        var userId = User.GetUserId();
        if (userId == -1)
            return NotFound();

        var result = await _contestService.GetHintMediaAsync(userId, id);
        return this.MapMediaServiceResult(result);
    }
}