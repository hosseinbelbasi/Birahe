using System.Security.Claims;

namespace Birahe.EndPoint.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value
               ?? user.FindFirst("unique_name")?.Value
               ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}