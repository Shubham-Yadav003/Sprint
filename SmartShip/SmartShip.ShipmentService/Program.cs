using Microsoft.EntityFrameworkCore;
using SmartShip.ShipmentService.Infrastructure.Data;
using SmartShip.ShipmentService.Application.Interfaces;
using SmartShip.ShipmentService.Application.Services;
// for jwt
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// exception handling
using SmartShip.ShipmentService.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
    });

// Register the context
builder.Services.AddDbContext<ShipmentDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the service
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddHttpClient();

// Add services to the container
//for enums as well

builder.Services.AddControllers()  // enum -> json 
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();

// for authorize option 
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // swagger page data
    app.UseSwaggerUI(); // swagger ui
}

app.UseHttpsRedirection(); // Adds middleware to intercept plain HTTP requests and redirect them to their secure HTTPS equivalent

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // for mapping to correct route  method >

app.Run();
