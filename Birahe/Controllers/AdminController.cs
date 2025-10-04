using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Repositories;
using Birahe.EndPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Controllers;

[ApiController]
[Route("/Admin/[action]")]
[Authorize(Roles = "admin")]
public class AdminController : Controller {
    private readonly AdminService _adminService;


    public AdminController( AdminService adminService) {
        _adminService = adminService;
    }

    [HttpPost]
    public async Task<IActionResult> AddRiddle([FromBody] RiddleDto riddleDto) {
        var result = await _adminService.AddRiddleAsync(riddleDto);
        if (!result.Success) {
            return result.Error switch
            {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound   => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new {
            success = result.Success,
            message = result.Message
        });
    }


    [HttpGet]
    public async Task<IActionResult> GetRiddles() {
        var result = await _adminService.GetRiddlesAsync();
        if (!result.Success) {
            return result.Error switch
            {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound   => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.NoContent => StatusCode(204, new {message = result.Message}),
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
    public async Task<IActionResult> EditRiddle([FromBody] RiddleDto riddleDto) {
        var result = await _adminService.EditRiddleAsync(riddleDto);
        if (!result.Success) {
            return result.Error switch
            {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound   => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
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
    public async Task<IActionResult> RemoveRiddle([FromBody] RemoveRiddleDto removeRiddleDto) {
        var result = await _adminService.DeleteRiddleAsync(removeRiddleDto);
        if (!result.Success) {
            return result.Error switch
            {
                ErrorType.Validation => BadRequest(new { message = result.Message }),
                ErrorType.NotFound   => NotFound(new { message = result.Message }),
                ErrorType.ServerError => StatusCode(500, new { message = result.Message }),
                ErrorType.NoContent => NoContent(),
                _ => BadRequest(new { message = result.Message })
            };
        }

        return Ok(new {
            success = result.Success,
            message = result.Message
        });
    }



}