using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Seed;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<SubastaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SubastaConnection")));


// Configuración Repositories
builder.Services.AddScoped<ISubastaRepository, SubastaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPujaRepository, PujaRepository>();
builder.Services.AddScoped<IBilleteraRepository, BilleteraRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Auth Service
builder.Services.AddScoped<IAuthService, AuthService>();


// Configuración de JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();


app.MapControllers();

//JWT
app.UseAuthentication();
app.UseAuthorization();


// Configuración de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Permite acceder a la interfaz en /swagger
}

//Seed de datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SubastaContext>();

    // Opcional: aplica migraciones pendientes automáticamente
    context.Database.Migrate();

    SeedData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
