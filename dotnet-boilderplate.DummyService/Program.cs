using dotnet_boilderplate.DummyService.Persistence;
using dotnet_boilderplate.ServiceDefaults.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Aspire auto-injects connection strings
builder.AddNpgsqlDbContext<DummyDbContext>("postgres");
builder.AddRedisClient("cache");
builder.AddRabbitMQClient("rabbitmq");

// 2. Add Default Service
builder.AddServiceDefaults();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
