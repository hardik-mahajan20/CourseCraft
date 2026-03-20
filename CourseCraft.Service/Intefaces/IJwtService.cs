using System.Security.Claims;

namespace CourseCraft.Service.Intefaces;

public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for the specified email.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="rememberMe">Indicates whether the token should have an extended expiration time.</param>
    /// <returns>A task that returns the generated JWT token as a string.</returns>
    Task<string> GenerateJwtTokenAsync(string email, bool rememberMe = false);

    /// <summary>
    /// Validates a JWT token and retrieves the claims principal.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>The claims principal if the token is valid, otherwise null.</returns>
    ClaimsPrincipal? ValidateToken(string token);
}
