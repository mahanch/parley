using Projects;

var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Parley_Api>("api");

builder.Build().Run();
