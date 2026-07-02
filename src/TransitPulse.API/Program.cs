using TransitPulse.Application;
using TransitPulse.Infrastructure;
using TransitPulse.API.Middleware;

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

app.MapControllers();

app.Run();