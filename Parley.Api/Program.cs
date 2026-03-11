using Microsoft.Extensions.Diagnostics.HealthChecks;
using Parley.Application._Bootstrapper;
using Parley.Infrastructure._Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (health checks, logging, service discovery, resilience)
// builder.AddServiceDefaults();
builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
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

// این هم health endpoint های Aspire رو map میکنه (/health و /alive)
app.MapDefaultEndpoints();

app.MapControllers();


app.Run();

