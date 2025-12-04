using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// 1. Aspire auto-injects connection strings
//builder.AddNpgsqlDbContext<OrderDbContext>("postgres");
builder.AddRedisClient("cache");
builder.AddRabbitMQClient("rabbitmq");

// 2. Services
builder.Services.AddHealthChecks()
       //.Add("postgres")
       //.AddRabbitMQ("rabbitmq")
       .AddRedis("cache");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapHealthChecks("/health");

app.MapGet("/", () => "OrderService.Api is running with Aspire 13.0 + .NET 10!");

app.MapGet("/test-redis", async (IConnectionMultiplexer redis) =>
{
    var db = redis.GetDatabase();
    await db.StringSetAsync("test", DateTime.Now.ToString(), TimeSpan.FromMinutes(10));
    var value = await db.StringGetAsync("test");
    return Results.Ok($"Redis working! Value = {value}");
});

app.Run();
