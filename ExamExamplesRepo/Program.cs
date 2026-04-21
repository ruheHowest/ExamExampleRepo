using ExamExamplesRepo.Domain.Interfaces;
using ExamExamplesRepo.Domain.Models;
using ExamExamplesRepo.Domain.Security;
using ExamExamplesRepo.Infrastruture.Data;
using ExamExamplesRepo.Infrastruture.Repositories;
using ExamExamplesRepo.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(); // Add Aspire Defaults

// Options Pattern for JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var secretKey = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

// Database context (connection string
builder.Services.AddDbContext<SchoolDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDb"));
});

// Dependency Injection (repos should be scoped)
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Jwt Auth setup
builder.Services
    .AddAuthentication(options =>
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
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero  // No tolerance on expiry
        };
});

builder.Services.AddAuthorizationBuilder()
                .AddPolicy("AnyUser", policy => policy.RequireRole(Roles.Admin, Roles.Manager, Roles.User));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(openApiOptions =>
{
    openApiOptions.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
using (IServiceScope scope = app.Services.CreateScope())
{
    SchoolDbContext dbContext = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    IUserRepository userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

    // Ensure the database schema is created before seeding
    dbContext.Database.EnsureCreated();

    bool hasAnyUsers = await dbContext.Users.AnyAsync();

    if (!hasAnyUsers)
    {
        User adminUser = new User
        {
            Name = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = Roles.Admin
        };

        User managerUser = new User
        {
            Name = "manager",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager123!"),
            Role = Roles.Manager
        };

        User regularUser = new User
        {
            Name = "user",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
            Role = Roles.User
        };

        await userRepository.AddAsync(adminUser);
        await userRepository.AddAsync(managerUser);
        await userRepository.AddAsync(regularUser);
    }
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "School API";
        options.Theme = ScalarTheme.Purple;
    }); // https://localhost:{port}/scalar/v1
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
