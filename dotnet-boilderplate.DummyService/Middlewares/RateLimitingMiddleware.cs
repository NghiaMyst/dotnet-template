using dotnet_boilderplate.ServiceDefaults.Contracts;

namespace dotnet_boilderplate.DummyService.Middlewares
{
    public class RateLimitingMiddleware(RequestDelegate next, IRateLimitService handler)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string clientId = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
            string key = $"rl_{clientId}";

            var isAllowed = await handler.IsAllowedAsync(
                key,
                limit: 10,
                period: TimeSpan.FromMinutes(1)
            );

            if (!isAllowed)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsJsonAsync(new { message = "Too many requests!" });
                return;
            }

            await next(context);
        }
    }
}
