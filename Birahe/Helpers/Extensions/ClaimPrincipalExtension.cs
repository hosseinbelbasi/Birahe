using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Birahe.EndPoint.Helpers.Extensions;

public static class ClaimsPrincipalExtensions {
    public static string? GetUsername(this ClaimsPrincipal user) {
        return user.FindFirst(ClaimTypes.Name)?.Value
               ?? user.FindFirst("unique_name")?.Value
               ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public static int GetUserId(this ClaimsPrincipal user) {
        var userIdClaim = user.FindFirst(JwtRegisteredClaimNames.NameId)?.Value
                          ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? user.FindFirst(ClaimTypes.Name)?.Value;
        if (int.TryParse(userIdClaim, out var id)) {
            return id;
        }

        return -1;
    }
}