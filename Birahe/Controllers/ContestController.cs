using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Birahe.EndPoint.DataBase;
using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Extensions;
using Birahe.EndPoint.Models.Dto.ContestDto_s;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;

[ApiController]
[Route("/contest/[action]")]
[Authorize]
public class ContestController : Controller {
    private readonly ContestService _contestService;


    public ContestController(ContestService contestService) {
        _contestService = contestService;

    }


    [HttpGet]
    public async Task<IActionResult> GetAllRiddles() {
        var currentUsername = User.GetUsername();
        if (String.IsNullOrEmpty(currentUsername)) {
            return BadRequest();
        }

        var result = await _contestService.GetAllRiddlesWithStatusAsync(currentUsername);

        if (!result.Success) {
            return result.Error switch {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.Forbidden => StatusCode(403, new{ message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new {
            success = result.Success,
            message = result.Message,
            data = result.Data
        });

    }

    [HttpGet]
    public async Task<IActionResult> OpenRiddle(int id) {
        var currentUsername = User.GetUsername();
        if (String.IsNullOrEmpty(currentUsername)) {
            return BadRequest();
        }

        var result = await _contestService.OpenRiddleAsync(currentUsername, id);

        if (!result.Success) {
            return result.Error switch {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.Forbidden => StatusCode(403, new{ message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new {
            success = result.Success,
            message = result.Message,
            data = result.Data
        });


    }

    [HttpGet]
    public async Task<IActionResult> OpenHint(int id) {
        var currentUsername = User.GetUsername();
        if (String.IsNullOrEmpty(currentUsername)) {
            return BadRequest();
        }

        var result = await _contestService.OpenRiddleHintAsync(currentUsername, id);

        if (!result.Success) {
            return result.Error switch {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.Forbidden => StatusCode(403, new{ message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new {
            success = result.Success,
            message = result.Message,
            data = result.Data
        });


    }

    [HttpPost]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDto submitAnswerDto) {
        var currentUsername = User.GetUsername();
        if (String.IsNullOrEmpty(currentUsername)) {
            return BadRequest();
        }

        var result = await _contestService.SubmitAnswerAsync(currentUsername, submitAnswerDto);

        if (!result.Success) {
            return result.Error switch {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.Forbidden => StatusCode(403, new{ message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new {
            success = result.Success,
            message = result.Message,
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetRiddle(int id) {
        var currentUsername = User.GetUsername();
        if (String.IsNullOrEmpty(currentUsername)) {
            return BadRequest();
        }

        var result = await _contestService.GetRiddleWithStatusAsync(id,currentUsername);

        if (!result.Success) {
            return result.Error switch {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.Forbidden => StatusCode(403, new{ message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new {
            success = result.Success,
            message = result.Message,
            data = result.Data
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetLeaderBoard() {
        var result = await _contestService.GetLeaderBoardAsync();

        if (!result.Success) {
            return result.Error switch {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.Forbidden => StatusCode(403, new{ message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new {
            success = result.Success,
            message = result.Message,
            data = result.Data
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetUserStatus() {
        throw new NotImplementedException();

    }

    // =================== Image Retrieval ========================

    [HttpGet]
    public async Task<IActionResult> GetRewardImage(int riddleId)
    {
        var username = User.GetUsername();
        if (string.IsNullOrEmpty(username))
            return BadRequest();

        var result = await _contestService.GetRewardImageAsync(username, riddleId);

        if (!result.Success)
        {
            return result.Error switch
            {
                ErrorType.Forbidden => Forbid(),
                ErrorType.NotFound => NotFound(new { message = result.Message }),
                _ => BadRequest(new { message = result.Message })
            };
        }

        var (bytes, contentType) = result.Data;
        return File(bytes, contentType);
    }

    [HttpGet]
    public async Task<IActionResult> GetHintImage(int riddleId) {
        var username = User.GetUsername();
        if (string.IsNullOrEmpty(username))
            return BadRequest();

        var result = await _contestService.GetHintImageAsync(username, riddleId);

        if (!result.Success)
        {
            return result.Error switch
            {
                ErrorType.Forbidden => Forbid(),
                ErrorType.NotFound => NotFound(new { message = result.Message }),
                _ => BadRequest(new { message = result.Message })
            };
        }

        var (bytes, contentType) = result.Data;
        return File(bytes, contentType);
    }




}

