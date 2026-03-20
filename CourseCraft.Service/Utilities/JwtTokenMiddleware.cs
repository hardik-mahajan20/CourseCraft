
using Microsoft.AspNetCore.Http;

namespace CourseCraft.Service.Utilities;

public class JwtTokenMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        string? token = CookieUtils.GetJWTToken(context.Request);

        if (!string.IsNullOrEmpty(token) && !context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }

        await _next(context);
    }
}
