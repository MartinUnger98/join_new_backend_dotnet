using JoinBackendDotnet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 🔌 DB-Konfiguration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=join.db"));

// 🌐 CORS erlauben (für Angular-Frontend z. B.)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🧭 Controller- und API-Dokumentation
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter(null, allowIntegerValues: false)
    );
});

builder.Services.AddEndpointsApiExplorer();

// 🧾 Swagger-Konfiguration inkl. Token-Auth
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Join API", Version = "v1" });

    // 🔐 Token-Auth hinzufügen (wie "Token xyz123")
    c.AddSecurityDefinition("Token", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "Token",
        Description = "Gib deinen Token ein: Token abc123...",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Token"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 🧠 CORS aktivieren
app.UseCors("AllowFrontend");

// 🧪 Swagger nur im Dev-Modus
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
