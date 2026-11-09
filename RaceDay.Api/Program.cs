using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using System.Security.Claims;
using System.Security.Principal;
using RaceDay.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<RaceDayDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RaceDayConnection")));

builder.Services.AddSingleton<IAzureBlobStorageService, AzureBlobStorageService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseRouting();

app.UseSession();


// Convert the API session values into claims for [Authorize].
app.Use(async (context, next) =>
{
    var userId = context.Session.GetInt32("UserId");
    var role = context.Session.GetString("Role");

    if (userId.HasValue && !string.IsNullOrWhiteSpace(role))
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "Session");
        context.User = new ClaimsPrincipal(identity);
    }

    await next();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "https://localhost:7257/openapi/v1.json",
            "RaceDay API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
