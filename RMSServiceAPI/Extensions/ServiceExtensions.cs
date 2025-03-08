using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using DomainLayer.Exceptions;
using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DataAccessLayer.Infrastructure.Repositories.RepoImplementations;
using ServicesLayer.ServiceInterface;
using ServicesLayer.ServiceImplementations;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using DomainLayer.Common;
using DomainLayer.Models.DomainModels;
using ServicesLayer.ServiceInterfaces;
using SendGrid;
using RMSServiceAPI.CustomMiddlewareExceptions;


namespace RMSServiceAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void CorsConfigurationRegister(this IServiceCollection services, IConfiguration configuration)
        {
            var clientUrl = configuration.GetSection("ApplicationBaseURLS:RMSClientUrl").Value;
            //var BaseURL = configuration.GetSection("ApplicationBaseURLS:RMSBaseUrl").Value;
            services.AddCors(options =>
            {
                options.AddPolicy(name : "AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins(clientUrl) // Replace with your frontend URL if different
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .WithExposedHeaders("Authorization");
                });
            });
        }

        public static void ServicesRegister(this IServiceCollection collection)
        {
            collection.AddHttpContextAccessor();

            collection.AddScoped<IUserAuthenticationRepo, UserAuthenticationRepo>();
            collection.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            collection.AddScoped<IHomepageRepo, HomePageRepo>();
            collection.AddScoped<IHomepageService, HomepageService>();
            collection.AddScoped<IMenuManagementRepo, MenuManagementRepo>();
            collection.AddScoped<IMenuManagementService, MenuManagementService>();
            collection.AddScoped<IOrderManagementRepo, OrderManagementRepo>();
            collection.AddScoped<IOrderManagementService, OrderManagementService>();
            collection.AddScoped<IEmailService, EmailService>();
            collection.AddSingleton<JwtAuthorizationFilter>();
        }

        public static void ConfigureSeriLogs(this IServiceCollection collection, IConfiguration configuration)
        {
            // Fetch the "SeriLog" section
            var serilogSection = configuration.GetSection("Logging:SeriLog");
            // Fetch values from the "SeriLog" section
            var logDirectory = serilogSection["LogDirectory"];
            var eventSourceName = serilogSection["EventSourceName"];
            var successLogFileName = serilogSection["SuccessLogFileName"];
            var errorLogFileName = serilogSection["ErrorLogFileName"];

            // Handle the case where configuration values might be null
            if (string.IsNullOrEmpty(logDirectory))
            {
                throw new CustomInvalidOperationException("LogDirectory configuration is missing or empty.");
            }
            if (string.IsNullOrEmpty(eventSourceName))
            {
                throw new CustomInvalidOperationException("EventSourceName configuration is missing or empty.");
            }
            // Handle the case where configuration values might be null
            if (string.IsNullOrEmpty(successLogFileName))
            {
                throw new CustomInvalidOperationException("successLogFileName configuration is missing or empty.");
            }
            if (string.IsNullOrEmpty(errorLogFileName))
            {
                throw new CustomInvalidOperationException("errorLogFileName configuration is missing or empty.");
            }
            // Ensure the log directory exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

         // Create a LoggerConfiguration instance
            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.Console()
                // Info log file - captures only Information level logs
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.File(
                        Path.Combine(logDirectory, successLogFileName),
                        rollingInterval: RollingInterval.Day
                    )
                )
                // Error log file - captures only Error level logs
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.File(
                        Path.Combine(logDirectory, errorLogFileName),
                        rollingInterval: RollingInterval.Day
                    )
                );


#if WINDOWS
                            // Add the EventLog sink only if compiling for Windows
                            loggerConfiguration = loggerConfiguration.WriteTo.EventLog(eventSourceName, restrictedToMinimumLevel: LogEventLevel.Information);
#endif

            Log.Logger = loggerConfiguration.CreateLogger();
        }

        public static void JWTAuthenticationRegister(this IServiceCollection collection, WebApplicationBuilder builder)
        {
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }

        public static void CustomDateTimeConverterRegister(this IServiceCollection collection, WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
                // No need for a custom DateTimeConverter if you use ISO 8601
            });
        }

        public static void SMTPConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>();
            services.AddSingleton<ISendGridClient>(new SendGridClient(smtpSettings.Password));
            services.AddHttpClient<IEmailService, EmailService>(client =>
            {
            });
        }
    }
}
