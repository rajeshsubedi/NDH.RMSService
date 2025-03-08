using DomainLayer.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RMSServiceAPI.CustomMiddlewareExceptions
{
    public class JwtAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public JwtAuthorizationFilter(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpcontext = _httpContextAccessor.HttpContext;
            // Retrieve the Authorization header from the request
            var authHeader = httpcontext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
            {
                // If Authorization header is missing, return 400 Bad Request
                throw new UserUnauthenticatedException("Invalid request: Missing Authorization header.");
            }

            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                // If Authorization header doesn't start with "Bearer ", return 401 Unauthorized
                httpcontext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpcontext.Response.WriteAsync("Unauthorized: Invalid Authorization header format. Expected 'Bearer <token>'.");
                return;
            }

            // Extract the token by removing "Bearer "
            var token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                // If token is empty after removing "Bearer ", return 401 Unauthorized
                httpcontext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpcontext.Response.WriteAsync("Unauthorized: Token is missing.");
                return;
            }

            // Validate the token
            if (!await ValidateTokenAsync(token))
            {
                // If the token is invalid, return 403 Forbidden
                httpcontext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpcontext.Response.WriteAsync("Forbidden: Invalid or expired token.");
                return;
            }
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
