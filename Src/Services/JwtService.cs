using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DebtTrack.Interfaces;
using DebtTrack.Models;
using DebtTrack.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DebtTrack.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;

  public JwtService(IOptions<JwtSettings> options)
{
    _settings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
}


    public string GenerateToken(UserModel user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}