using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Birahe.EndPoint.Helpers.Extensions;

public static class ExceptionHandlerExtensions {
    public static void UseProblemDetailsExceptionHandler(this IApplicationBuilder app) {
        app.UseExceptionHandler(appBuilder => {
            appBuilder.Run(async context => {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exceptionMessage = exceptionHandlerFeature?.Error.Message ?? "An unexpected error occurred.";

                var problem = new ProblemDetails {
                    Type = "https://birahe.com/Errors/server-error",
                    Title = "An unexpected error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exceptionMessage, // optional: can keep generic if you don't want real error exposed
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problem);
            });
        });
    }
}