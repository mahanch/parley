using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (health checks, logging, service discovery, resilience)
// builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Map service defaults (health check endpoints)
// app.MapDefaultEndpoints();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.MapGet("/health/ready", async (IHealthCheckService healthCheck) =>
// {
//     var result = await healthCheck.CheckHealthAsync();
//     return result.Status == HealthStatus.Healthy ? Results.Ok() : Results.StatusCode(503);
// });

app.Run();

