using dotnet_boilderplate.ServiceDefaults.Extensions;
using dotnet_template.AuthService.Features.Commands.GetUsers;
using dotnet_template.AuthService.Features.Commands.LoginWithPassword;
using dotnet_template.AuthService.Features.Commands.RegisterUser;
using dotnet_template.AuthService.Persistence;
using dotnet_template.AuthService.Persistence.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //ValidAudience = builder.Configuration["Jwt:Audience"],
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
            ValidIssuer = "_nghiant",
            ValidAudience = "_nghiant",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7p9zBvE6xN8mK2qR5wL4nH1jG3sA9dB7eC0uI2oP5tY="))
        };
    });

builder.AddCustomAuthorization();

// Config Validator
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginWithPasswordValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetUsersValidator>();

// Config Handler
builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginWithPasswordHandler>();
builder.Services.AddScoped<GetUsersHandler>();

builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AuthService API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization using Bearer Scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// Map endpoint
app.MapRegisterUserEndpoint();
app.MapLoginWithPassword();
app.MapGetUsers();

app.Run();
