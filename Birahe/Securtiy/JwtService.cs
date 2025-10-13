using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Birahe.EndPoint.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Birahe.EndPoint.Services;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        // 1) Secret key
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 2) Claims
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString().ToLower()) // role for [Authorize(Roles="...")]
        };

        // 3) Token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"], // set in appsettings.json
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_config["Jwt:DurationInMinutes"] ?? "30")),
            signingCredentials: creds
        );

        // 4) Return raw JWT string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}