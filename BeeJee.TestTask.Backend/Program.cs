global using BeeJee.TestTask.Backend.Common;
using BeeJee.TestTask.Backend.Config;
using BeeJee.TestTask.Backend.Dto;
using BeeJee.TestTask.Backend.Helpers;
using BeeJee.TestTask.Backend.Middleware;
using BeeJee.TestTask.Backend.Services;
using BeeJee.TestTask.Backend.Validation;
using BeeJee.TestTask.DAL;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var configFilePath = PathHelper.GetConfigPath();
Console.WriteLine($"configFilePath={configFilePath}");
builder.Configuration.AddJsonFile(configFilePath);

builder.Services.AddDbContext<TestTaskDbContext>(options =>
{
    var dbOptions = builder.Configuration.GetSection("DB").Get<DbOptions>();
    var dbDataSource = Path.Combine(dbOptions.SqliteDbPath ?? "");
    Console.WriteLine($"dbDataSporce={dbOptions.SqliteDbPath}");

    var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = dbDataSource };

    Console.WriteLine($"connectionString={connectionStringBuilder}");
    Console.WriteLine($"Is DB file existst = {File.Exists(dbDataSource)}");

    options.UseSqlite(connectionStringBuilder.ToString());
});

var authOptions = builder.Configuration.GetSection("AuthOptions").Get<AuthOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = authOptions.GetTokenValidationParameters();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer yourToken12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddScoped<IValidator<NewTaskDto>, NewTaskValidator>();
builder.Services.AddScoped<IValidator<LoginRequestDto>, LoginValidator>();
builder.Services.Configure<PageOptions>(builder.Configuration.GetSection("PageOptions"));
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthOptions"));
builder.Services.AddSingleton<ITokenService, TokenService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TestTaskDbContext>();
    var wasCreated = dbContext.Database.EnsureCreated();
}

app.Run();
