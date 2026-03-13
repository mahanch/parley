using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Parley.Api.Filters;
using Parley.Api.Middlewares;
using Parley.Application._Bootstrapper;
using Parley.Infrastructure._Bootstrapper;
using Parley.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (health checks, logging, service discovery, resilience)
// builder.AddServiceDefaults();
builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// Add services to the container
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers(options =>
    options.Filters.Add<BaseResponseFilter>());

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


app.UseExceptionHandler();

// این هم health endpoint های Aspire رو map میکنه (/health و /alive)
app.MapDefaultEndpoints();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ParleyDbContext>();
    await db.Database.MigrateAsync();
}
app.Run();

