using TransitPulse.Application;
using TransitPulse.Infrastructure;
using TransitPulse.API.Middleware;
using TransitPulse.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ASP.NET Core framework
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Application layer
builder.Services.AddApplicationServices();

// Infrastructure layer
builder.Services.AddInfrastructureServices(
    builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Database seeding
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider
        .GetRequiredService<DbSeeder>();

    await seeder.SeedAsync();
}

app.MapControllers();

app.Run();