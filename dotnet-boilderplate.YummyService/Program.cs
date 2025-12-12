using dotnet_boilderplate.ServiceDefaults.Extensions;
using dotnet_boilderplate.YummyService.Messaging;

var builder = WebApplication.CreateBuilder(args);
var rabbitmqConn = builder.Configuration.GetConnectionString("rabbitmq");

builder.AddServiceDefaults();
builder.AddRabbitMQClient(rabbitmqConn);

builder.Services.AddHostedService<RmqDomainEventConsumer>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
