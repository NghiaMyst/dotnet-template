var builder = DistributedApplication.CreateBuilder(args);


#region SharedConfigs
// Database Configuration
var postgres = builder.AddConnectionString("postgres");

// Message Queue
var rabbitmq = builder.AddConnectionString("rabbitmq");

// Redis
var redis = builder.AddConnectionString("redis");
#endregion

#region ProjectsReference
builder.AddProject<Projects.dotnet_boilderplate_DummyService>("dotnet-boilderplate-dummyservice")
    .WithReference(rabbitmq)
    .WithReference(postgres)
    .WithReference(redis);

builder.AddProject<Projects.dotnet_boilderplate_YummyService>("dotnet-boilderplate-yummyservice")
    .WithReference(rabbitmq);
#endregion

builder.Build().Run();
