using SchoolManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolManagementSystem.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Enums;


var builder = WebApplication.CreateBuilder(args);

// 1. Register DbContext with MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);


// 2. Register JwtService -> so it can be injected into AuthController
builder.Services.AddScoped<JwtService>();

// 3. JWT Authentication

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("Jwt:Key is missing from appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options =>
    {
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();


// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "School Management Systen API",
        Version = "v1"
    });

    // adds the Authorize button in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token here."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });


});

var app = builder.Build();


// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "School Management System v1");
        c.RoutePrefix = string.Empty;  //Swagger opens at rool URL
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// Seed password is loaded from appsettings.Development.json
var adminPassword = builder.Configuration["Seed:AdminPassword"]
    ?? throw new Exception("Seed:AdminPassword not configured");

// Seed first Admin user on first startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<SchoolDbContext>();


    // Only runs if no users exist in the database
    if (!context.Users.Any())
    {
        context.Users.Add(new User
        {
            UserName = "superadmin",
            Password = BCrypt.Net.BCrypt.HashPassword(adminPassword), // password from config
            Email = "admin@school.lk",
            Role = UserRole.Admin,
            Status = UserStatus.Active,
            CreateDate = DateTime.UtcNow
        });
        context.SaveChanges();
        Console.WriteLine("Super admin seeded successfully.");
    }
}

app.Run();

