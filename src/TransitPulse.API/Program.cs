using TransitPulse.Application;
using TransitPulse.Infrastructure;
using TransitPulse.API.Middleware;
using TransitPulse.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ASP.NET Core framework
builder.Services.AddControllers(); //Registers the Web API framework.
builder.Services.AddOpenApi(); //Registers the OpenAPI documentation services.

// Application layer
builder.Services.AddApplicationServices(); // Registers your business logic

// Infrastructure layer
builder.Services.AddInfrastructureServices(builder.Configuration); //Registers database, repositories, and infrastructure services.



var app = builder.Build();



app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 👇 Run the database seeder here
//
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await seeder.SeedAsync();
}

app.MapControllers();

app.Run();