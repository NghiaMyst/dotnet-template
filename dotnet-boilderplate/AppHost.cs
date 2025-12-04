var builder = DistributedApplication.CreateBuilder(args);


#region SharedConfigs
// Database Configuration
var postgres = builder.AddPostgres("postgres")
                        .WithImage("postgres", "17");

// Message Queue
var rabbitmq = builder.AddRabbitMQ("rabbitmq")
       .WithImage("rabbitmq", "3-management");

// Redis
var redis = builder.AddRedis("cache")
    .WithImage("redis", "7-alpine");
#endregion

#region ProjectsReference
builder.AddProject<Projects.dotnet_boilderplate_DummyService>("dotnet-boilderplate-dummyservice")
    .WithReference(rabbitmq)
    .WithReference(postgres)
    .WithReference(redis);
#endregion

builder.Build().Run();
