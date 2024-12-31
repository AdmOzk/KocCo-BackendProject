using KocCoAPI.API.Mapping;
using KocCoAPI.Application.Interfaces;
using KocCoAPI.Application.Services;
using KocCoAPI.Domain.Interfaces;
using KocCoAPI.Domain.Services;
using KocCoAPI.Infrastructure.Persistence;
using KocCoAPI.Infrastructure.Repositories;
using KocCoAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// *CORS Configuration*
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()    // Allow requests from any origin
            .AllowAnyMethod()    // Allow all HTTP methods
            .AllowAnyHeader());  // Allow all headers
});

// *Database Context*
builder.Services.AddDbContext<SqlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// *Service Registrations*
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// *Swagger Configuration*
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KocCoAPI",
        Version = "v1",
        Description = "API documentation for KocCo application",
        Contact = new OpenApiContact
        {
            Name = "Support Team",
            Email = "support@example.com",
        }
    });

    // JWT Authentication in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// *Application Services*
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAppService, UserAppService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartAppService, CartAppService>();



builder.Services.AddScoped<IPackageAppService, PackageAppService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();




// *Authentication and Authorization Services*
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddAutoMapper(typeof(MappingProfile));

// *JWT Authentication Configuration*
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = jwtSettings["Key"]; // JWT secret key from appsettings.json

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,   // Disable issuer validation for simplicity
        ValidateAudience = false, // Disable audience validation for simplicity
        ValidateLifetime = true,  // Ensure the token hasn't expired
        ClockSkew = TimeSpan.Zero, // No extra time on token expiration
        RoleClaimType = ClaimTypes.Role // Rol doðrulamasý için burasý önemli
    };
});

// *Application Build*
var app = builder.Build();

// *Configure Middleware Pipeline*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// *CORS Middleware*
app.UseCors("AllowAllOrigins");

// *Authentication and Authorization Middleware*
app.UseAuthentication();
app.UseAuthorization();

// *Controller Mapping*
app.MapControllers();

app.Run();
