namespace RMSServiceAPI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void CorsMiddlewareRegister(this WebApplication app)
        {
          app.UseCors("AllowSpecificOrigin");
        }
    }
}
