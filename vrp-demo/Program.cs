using dotnet_boilderplate.ServiceDefaults.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Endpoints;
using vrp_demo.Features.Commands.Drivers.AddSkillToDriver;
using vrp_demo.Features.Commands.Drivers.CreateDriver;
using vrp_demo.Features.Commands.Drivers.DeleteDriver;
using vrp_demo.Features.Commands.Drivers.UpdateDriver;
using vrp_demo.Features.Commands.Jobs.CreateJob;
using vrp_demo.Features.Commands.Skills.CreateSkill;
using vrp_demo.Features.Queries.Drivers.GetDriver;
using vrp_demo.Features.Queries.Skills.GetSkills;
using vrp_demo.Persistence;
using vrp_demo.Persistence.Services;

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

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new NetTopologySuite.IO.Converters.GeoJsonConverterFactory());

    options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
});

// 2. Add Default Service
builder.AddServiceDefaults();

// 3. Validation
builder.Services.AddValidatorsFromAssemblyContaining<CreateSkillValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetSkillsValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDriverValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateJobValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddSkillToDriverValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateDriverValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetDriverValidator>();

// 4. Hanlder
builder.Services.AddScoped<JobCodeGenerator>();
builder.Services.AddScoped<CreateSkillHandler>();
builder.Services.AddScoped<GetSkillsHandler>();
builder.Services.AddScoped<CreateDriverHandler>();
builder.Services.AddScoped<CreateJobHandler>();
builder.Services.AddScoped<AddSkillToDriverHandler>();
builder.Services.AddScoped<DeleteDriverHandler>();
builder.Services.AddScoped<UpdateDriverHandler>();
builder.Services.AddScoped<GetDriverHandler>();

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
app.MapDriverEndpoints();
app.MapJobsEndpoints();

app.Run();
