using dotnet_boilderplate.ServiceDefaults.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Features.Commands.Skills.CreateSkill;
using vrp_demo.Persistence;

var builder = WebApplication.CreateBuilder(args);

// 1. Aspire auto-injects connection strings
var postgresConn = builder.Configuration.GetConnectionString("postgres");

builder.Services.AddDbContext<VrpDbContext>(options =>
{
    options.UseNpgsql(postgresConn);
});

// 2. Add Default Service
builder.AddServiceDefaults();

// 3. Validation
builder.Services.AddValidatorsFromAssemblyContaining<CreateSkillValidator>();

// 4. Hanlder
builder.Services.AddScoped<CreateSkillHandler>();

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

app.Run();
