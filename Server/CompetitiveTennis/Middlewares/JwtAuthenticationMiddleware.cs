namespace CompetitiveTennis.Middlewares;

using Microsoft.AspNetCore.Http;
using Services.Interfaces;

using static Constants;

public class JwtAuthenticationMiddleware : IMiddleware
{
    private readonly ICurrentTokenService _token;

    public JwtAuthenticationMiddleware(ICurrentTokenService token) => _token = token;
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers[AuthorizationHeaderName].ToString();

        if (!string.IsNullOrWhiteSpace(token))
            _token.Set(token.Split().Last());

        await next.Invoke(context);
    }
}