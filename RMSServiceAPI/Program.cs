using DataAccessLayer.Infrastructure.Data;
using DomainLayer.Common;
using Microsoft.EntityFrameworkCore;
using RMSServiceAPI.CustomMiddlewareExceptions;
using RMSServiceAPI.Extensions;
using System.ComponentModel;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Services.ConfigureSeriLogs(builder.Configuration);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register SMTP configuration
builder.Services.SMTPConfigureService(builder.Configuration);

// Configure JWT Authentication, CORS, and other services
builder.Services.JWTAuthenticationRegister(builder);

builder.Services.CorsConfigurationRegister(builder.Configuration);

builder.Services.ServicesRegister();

// Register the DbContext with the connection string
builder.Services.AddDbContext<RMSServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.CustomDateTimeConverterRegister(builder);
// Register AutoMapper with the DI container
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();

app.CorsMiddlewareRegister();

app.UseHttpsRedirection();

app.UseMiddleware<JwtMiddleware>();
// Register custom exception handling middleware
app.UseMiddleware<CustomExceptionHandlingMiddleware>();

// Register CORS

// Use authorization and routing
app.UseAuthorization();
// Define custom route template
app.UseRouting();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
