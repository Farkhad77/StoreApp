using StoreApp.Application.Abstracts.Services;

namespace StoreApp.WebApi.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IRedisCacheService redis)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var isBlacklisted = await redis.ExistsAsync($"blacklist:access:{token}");
                if (isBlacklisted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token is blacklisted");
                    return;
                }
            }

            await _next(context);
        }
    }

}
