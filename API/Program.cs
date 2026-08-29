using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;
using Persistence;
using Application;
using Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Application.Contracts.UserManagement;
using Identity.Utils;
using Persistence.Repositories.UserManagement;
using Persistence.Seeders.UserManagement;
using Domain.Models.UserManagement;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// ===================== BASIC SERVICES =====================
builder.Services.AddHttpContextAccessor();

// ===================== LOCALIZATION =====================
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("ps-AF"),
        new CultureInfo("fa"),
        new CultureInfo("en")
    };

    options.DefaultRequestCulture = new RequestCulture("ps-AF");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});


// ===================== LAYERED ARCHITECTURE =====================
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureIdentityServices();

builder.Services.AddScoped<IPermissionSeeder, PermissionSeeder>();
builder.Services.AddScoped<IRoleSeeder, RoleSeeder>();
builder.Services.AddScoped<IUserSeeder, UserSeeder>();
builder.Services.AddScoped<IRolePermissionSeeder, RolePermissionSeeder>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


// ===================== REDIS CONFIGURATION =====================
var redisConnection = builder.Configuration.GetValue<string>("Redis:ConnectionString");

// Redis connection (Singleton)
//builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
//{
//    return ConnectionMultiplexer.Connect(redisConnection);
//});

// Distributed cache using Redis
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = redisConnection;
//});

// ===================== CONTROLLERS & CORS =====================
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3007",
                "http://localhost:5173",
                "https://localhost:5173",
                "http://127.0.0.1:5173",
                "https://127.0.0.1:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ===================== AUTHENTICATION (JWT) =====================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true,
        };
    });

// ===================== AUTHORIZATION (PERMISSION POLICIES) =====================
builder.Services.AddAuthorization(options =>
{
    var permissions = typeof(SystemClaims)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        .Where(f => f.IsLiteral && !f.IsInitOnly)
        .Select(f => f.GetValue(null)?.ToString())
        .Where(v => !string.IsNullOrWhiteSpace(v));

    foreach (var permission in permissions!)
    {
        options.AddPolicy(permission!, policy =>
        {
            policy.RequireClaim("permission", permission!);
        });
    }
});
// ===================== SWAGGER =====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
// ===================== DATABASE SEEDING =====================
using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider
            .GetRequiredService<IPermissionSeeder>()
            .SeedAsync();

    await scope.ServiceProvider
        .GetRequiredService<IRoleSeeder>()
        .SeedAsync();

    await scope.ServiceProvider
        .GetRequiredService<IRolePermissionSeeder>()
        .SeedAsync();

    await scope.ServiceProvider
        .GetRequiredService<IUserSeeder>()
        .SeedAsync();
}

// ===================== MIDDLEWARE =====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
