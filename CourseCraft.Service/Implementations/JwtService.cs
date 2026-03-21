using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CourseCraft.Repository.Interfaces;
using CourseCraft.Repository.Models;
using CourseCraft.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CourseCraft.Service.Implementations;

public class JwtService(IConfiguration configuration, IUsersRepository userRepository) : IJwtService
{
    private readonly string _key = configuration["Jwt:Key"]
            ?? throw new ArgumentNullException(nameof(configuration), "JWT key is missing in configuration.");
    private readonly string _issuer = configuration["Jwt:Issuer"]
            ?? throw new ArgumentNullException(nameof(configuration), "JWT issuer is missing in configuration.");
    private readonly string _audience = configuration["Jwt:Audience"]
            ?? throw new ArgumentNullException(nameof(configuration), "JWT audience is missing in configuration.");
    private readonly IUsersRepository _userRepository = userRepository;

    #region  GenerateJwtToken

    public async Task<string> GenerateJwtTokenAsync(string userEmail, bool rememberMe = false)
    {
        JwtSecurityTokenHandler? tokenHandler = new();
        byte[]? key = Encoding.UTF8.GetBytes(_key);

        User user = await _userRepository.GetAllUserAsQueryable().FirstOrDefaultAsync(u => u.UserEmail.ToLower() == userEmail.ToLower())
            ?? throw new Exception("User not found while generating JWT token.");


        List<Claim>? claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, userEmail),
            new Claim("username", user.UserName),
            new Claim(ClaimTypes.Email, user.UserEmail),
            new Claim(ClaimTypes.Role, user.UserRole),
        ];

        if (rememberMe)
        {
            claims.Add(new Claim("RememberMe", "True"));
        }

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    #endregion


    #region  ValidatToken

    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        JwtSecurityTokenHandler tokenHandler = new();
        byte[]? key = Encoding.UTF8.GetBytes(_key);
        try
        {
            TokenValidationParameters validationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    #endregion

}
