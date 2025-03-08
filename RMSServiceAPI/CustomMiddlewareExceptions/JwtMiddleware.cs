using DomainLayer.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RMSServiceAPI.CustomMiddlewareExceptions
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Retrieve the Authorization header from the request
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
            {
                // If Authorization header is missing, return 400 Bad Request
                throw new UserUnauthenticatedException("Invalid request: Missing Authorization header.");
            }

            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                // If Authorization header doesn't start with "Bearer ", return 401 Unauthorized
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid Authorization header format. Expected 'Bearer <token>'.");
                return;
            }

            // Extract the token by removing "Bearer "
            var token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                // If token is empty after removing "Bearer ", return 401 Unauthorized
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Token is missing.");
                return;
            }

            // Validate the token
            if (!await ValidateTokenAsync(token))
            {
                // If the token is invalid, return 403 Forbidden
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Invalid or expired token.");
                return;
            }

            // Proceed with the next middleware if everything is valid
            await _next(context);
        }



        private async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero, // Allow the token to expire immediately if it's expired
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Token validation failed: {ex.Message}");
                return false;
            }
        }
    }
}
