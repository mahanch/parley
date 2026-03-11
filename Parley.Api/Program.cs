using Microsoft.Extensions.Diagnostics.HealthChecks;
using Parley.Application._Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (health checks, logging, service discovery, resilience)
// builder.AddServiceDefaults();

builder.Services.AddApplication();

// Add services to the container
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Map service defaults (health check endpoints)
// app.MapDefaultEndpoints();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
// app.MapGet("/health/ready", async (IHealthCheckService healthCheck) =>
// {
//     var result = await healthCheck.CheckHealthAsync();
//     return result.Status == HealthStatus.Healthy ? Results.Ok() : Results.StatusCode(503);
// });

app.Run();

