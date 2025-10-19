using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.ResultModels;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Helpers.Extensions;

public static class ServiceResultHelper {
    public static IActionResult MapServiceResult<T>(this ControllerBase controller, ServiceResult<T> result) {
        if (result.Success)
            return controller.Ok(new { success = true, message = result.Message, data = result.Data });

        var statusCode = result.Error switch
        {
            ErrorType.Validation   => StatusCodes.Status400BadRequest,
            ErrorType.NotFound     => StatusCodes.Status404NotFound,
            ErrorType.Forbidden    => StatusCodes.Status403Forbidden,
            ErrorType.ServerError  => StatusCodes.Status500InternalServerError,
            ErrorType.NoContent    => StatusCodes.Status204NoContent,
            _                      => StatusCodes.Status400BadRequest
        };

        var problem = new ProblemDetails
        {
            Type = $"https://birahe.com/errors/{result.Error.ToString().ToLower()}",
            Title = result.Message,
            Status = statusCode,
            Detail = result.Detail ?? "An error occurred.",
            Instance = controller.HttpContext.Request.Path
        };

        if (statusCode == StatusCodes.Status204NoContent)
            return controller.NoContent();

        return new ObjectResult(problem)
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }

    public static IActionResult MapServiceResult(this ControllerBase controller, ServiceResult result) {
        if (result.Success)
            return controller.Ok(new { success = true, message = result.Message });

        var statusCode = result.Error switch
        {
            ErrorType.Validation   => StatusCodes.Status400BadRequest,
            ErrorType.NotFound     => StatusCodes.Status404NotFound,
            ErrorType.Forbidden    => StatusCodes.Status403Forbidden,
            ErrorType.ServerError  => StatusCodes.Status500InternalServerError,
            ErrorType.NoContent    => StatusCodes.Status204NoContent,
            _                      => StatusCodes.Status400BadRequest
        };

        var problem = new ProblemDetails
        {
            Type = $"https://birahe.com/errors/{result.Error.ToString().ToLower()}",
            Title = result.Message,
            Status = statusCode,
            Detail = result.Detail ?? "An error occurred.",
            Instance = controller.HttpContext.Request.Path
        };

        if (statusCode == StatusCodes.Status204NoContent)
            return controller.NoContent();

        return new ObjectResult(problem)
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }


    public static IActionResult MapImageServiceResult(this ControllerBase controller,
        ServiceResult<(byte[] bytes, string contentType)> result) {
        if (result.Success) {
            var (bytes, contentType) = result.Data;
            return controller.File(bytes, contentType);
        }

        return result.Error switch {
            ErrorType.Forbidden => controller.Forbid(),
            ErrorType.NotFound => controller.NotFound(new ProblemDetails {
                Title = "Resource not found",
                Status = StatusCodes.Status404NotFound,
                Detail = result.Message,
                Instance = controller.HttpContext.Request.Path
            }),
            _ => controller.BadRequest(new ProblemDetails {
                Title = "Unable to retrieve resource",
                Status = StatusCodes.Status400BadRequest,
                Detail = result.Message,
                Instance = controller.HttpContext.Request.Path
            })
        };
    }
}