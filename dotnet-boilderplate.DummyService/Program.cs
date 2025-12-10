using dotnet_boilderplate.DummyService.Features.Commands.CreateOrder;
using dotnet_boilderplate.DummyService.Features.Queries.GetOrderById;
using dotnet_boilderplate.DummyService.Persistence;
using dotnet_boilderplate.ServiceDefaults.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Aspire auto-injects connection strings
var postgresConn = builder.Configuration.GetConnectionString("postgres");
var redisConn = builder.Configuration.GetConnectionString("redis");
var rabbitmqConn = builder.Configuration.GetConnectionString("rabbitmq");

builder.Services.AddDbContext<DummyDbContext>(options =>
{
    options.UseNpgsql(postgresConn);
});

builder.AddRedisClient(redisConn);
builder.AddRabbitMQClient(rabbitmqConn);


// 2. Add Default Service
builder.AddServiceDefaults();

// 3. Validation
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetOrderByIdValidator>();

// 4. Hanlder
builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<GetOrderByIdHandler>();

builder.Services.AddSwaggerGen();

// 5. Handle Format
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

        options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None;
    });

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseSwagger();
app.UseSwaggerUI();

app.MapCreateOrderEndpoint();
app.MapGetOrderByIdEndpoint();

app.Run();
