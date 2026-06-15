using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Interfaces;
using TransitPulse.Infrastructure.Repositories;
using TransitPulse.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IRouteEventRepository, InMemoryRouteEventRepository>();

builder.Services.AddSingleton<IReliabilitySnapshotRepository, InMemoryReliabilitySnapshotRepository>();

builder.Services.AddSingleton<IReliabilityCalculator, ReliabiltyCalculator>();

builder.Services.AddScoped<GenerateReliabilitySnapshotHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();