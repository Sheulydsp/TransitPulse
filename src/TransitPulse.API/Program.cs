using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Interfaces;
using TransitPulse.Infrastructure.Repositories;
using TransitPulse.Infrastructure.Services;
using TransitPulse.API.Middleware;
using Microsoft.EntityFrameworkCore;
using TransitPulse.Infrastructure.Persistence;
using TransitPulse.Domain.Entities;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddDbContext<TransitPulseDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("TransitPulseDb")));

builder.Services.AddScoped<IRouteEventRepository, RouteEventRepository>();

builder.Services.AddScoped<IReliabilitySnapshotRepository, ReliabilitySnapshotRepository>();


builder.Services.AddSingleton<IReliabilityCalculator, ReliabiltyCalculator>();
builder.Services.AddScoped<GetReliabilitySnapshotsHandler>();

builder.Services.AddScoped<GenerateReliabilitySnapshotHandler>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();