
using System.Text;
using Kosa.ECommerce.Application.DependencyInjection;
using Kosa.ECommerce.Infrastructure.DependencyInjection;
using Kosa.ECommerce.Persistence.DependencyInjection;
using Kosa.ECommerce.Server.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Serilog
// ==========================================
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

// ==========================================
// 2. Dependency Injection — layered registration
// ==========================================
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

// ==========================================
// 3. CORS — Angular development origin only
// ==========================================
const string frontendCorsPolicy = "KosaFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(frontendCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ==========================================
// 4. JWT Authentication
// ==========================================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// ==========================================
// 5. MVC + OpenAPI
// ==========================================
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// ==========================================
// 6. HTTP Request Logging
// ==========================================
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ==========================================
// 7. Global Exception Handling
// ==========================================
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// ==========================================
// 8. Static Files
// ==========================================
app.UseStaticFiles();

// ==========================================
// 9. CORS
// ==========================================
app.UseCors(frontendCorsPolicy);

// ==========================================
// 10. Authentication & Authorization
// ==========================================
app.UseAuthentication();
app.UseAuthorization();

// ==========================================
// 11. Controllers
// ==========================================
app.MapControllers();

app.Run();
