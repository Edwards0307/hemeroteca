using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hemeroteca.API.Common;
using Hemeroteca.API.Repositories;
using Hemeroteca.API.Repositories.Interfaces;
using Hemeroteca.API.Services;
using Hemeroteca.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Conexión a la base de datos
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// Repositories
builder.Services.AddScoped<ILibroRepository, LibroRepository>();
builder.Services.AddScoped<IRevistaRepository, RevistaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Services
builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<IRevistaService, RevistaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "hemeroteca_secret_key_2024_segura";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
