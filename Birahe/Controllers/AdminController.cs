using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
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
    public async Task<IActionResult> AddRiddle([FromBody] AdminRiddleDto adminRiddleDto) {
        var result = await _adminService.AddRiddleAsync(adminRiddleDto);
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
    public async Task<IActionResult> GetAllRiddles() {
        var result = await _adminService.GetAllRiddlesAsync();
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
    public async Task<IActionResult> EditRiddle([FromBody] AdminRiddleDto adminRiddleDto) {
        var result = await _adminService.EditRiddleAsync(adminRiddleDto);
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

    [HttpGet]
    public async Task<IActionResult> GetAllUsers() {
        var result = await _adminService.GetAllUsersAsync();
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
    public async Task<IActionResult> BanUser([FromBody] BanUserDto banUserDto) {
        var result = await _adminService.BanUserAsync(banUserDto);
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