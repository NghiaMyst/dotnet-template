using dotnet_boilderplate.ServiceDefaults.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Endpoints;
using vrp_demo.Features.Commands.Drivers.CreateDriver;
using vrp_demo.Features.Commands.Skills.CreateSkill;
using vrp_demo.Features.Queries.Skills.GetSkills;
using vrp_demo.Persistence;

var builder = WebApplication.CreateBuilder(args);

// 1. Aspire auto-injects connection strings
var postgresConn = builder.Configuration.GetConnectionString("postgres");

builder.Services.AddDbContext<VrpDbContext>(options =>
{
    options.UseNpgsql(postgresConn, x =>
    {
        x.UseNetTopologySuite();
    });
});

// 2. Add Default Service
builder.AddServiceDefaults();

// 3. Validation
builder.Services.AddValidatorsFromAssemblyContaining<CreateSkillValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetSkillsValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDriverValidator>();

// 4. Hanlder
builder.Services.AddScoped<CreateSkillHandler>();
builder.Services.AddScoped<GetSkillsHandler>();

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

// 6. Endpoints
app.MapSkillsEndpoints();

app.Run();
