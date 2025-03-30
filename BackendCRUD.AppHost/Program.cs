var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.BackendCRUD_ApiService>("apiservice");

builder.AddProject<Projects.BackendCRUD_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
