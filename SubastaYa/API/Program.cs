using API.Hubs;
using API.Middlewares;
using API.Services;
using Application;
using Application.Interfaces;
using Application.UseCases.Billeteras;
using Application.UseCases.Finalizacion;
using Application.UseCases.Pujas;
using Application.UseCases.Subastas;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Seed;
using Infrastructure.Services;
using Infrastructure.Workers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esto hace que cualquier enum viaje como texto en el JSON
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


// Registro automático de TODOS los Casos de Uso de Application
builder.Services.AddApplicationServices();


// Registro de Casos de Uso de Billetera
builder.Services.AddScoped<ObtenerBilletera>();
builder.Services.AddScoped<ObtenerMovimientos>();
builder.Services.AddScoped<DepositarFondos>();

// Registrar el Caso de Uso de CrearSubasta
builder.Services.AddScoped<CrearSubasta>();

builder.Services.AddScoped<FinalizarSubastasExpiradas>();

builder.Services.AddScoped<ObtenerSubastasProximas>();


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
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Auth Service
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<EliminarSubasta>();
builder.Services.AddScoped<ObtenerMisPujas>();
builder.Services.AddScoped<ObtenerMisPublicaciones>();

builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();

// 1. Obtener clave secreta del appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

// 2. Registrar servicios de Autenticación JWT
builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.RequireHttpsMetadata = false; // Cambiar a true en producción
options.SaveToken = true;
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtSettings["Issuer"],
    ValidAudience = jwtSettings["Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
    ClockSkew = TimeSpan.Zero,
    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
};


    // 3. INTERCEPTOR CRÍTICO PARA SIGNALR (WebSockets)
    options.Events = new JwtBearerEvents
 {
    OnMessageReceived = context =>
     {
       var accessToken = context.Request.Query["access_token"];

       // Si la petición va hacia el Hub de SignalR, extraer el token del QueryString
       var path = context.HttpContext.Request.Path;
         if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/subasta"))
          {
            context.Token = accessToken;
          }
          return Task.CompletedTask;
        }
  };
});

builder.Services.AddAuthorization();

builder.Services.AddSignalR();
builder.Services.AddScoped<INotificadorSubasta, SignalNotificadorSubasta>();

// Registrar el Worker Service en segundo plano
builder.Services.AddHostedService<SubastaWorker>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Front", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});


var app = builder.Build();

//JWT
app.UseAuthentication();

// Endpoint Routing de API y Hubs
app.MapControllers();
app.MapHub<SubastaHub>("/hubs/subasta");

// 1. REGISTRAR TU MIDDLEWARE AL INICIO DEL PIPELINE
app.UseMiddleware<CorporateApiMiddleware>();

// Configuración de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Permite acceder a la interfaz en /swagger
}

// ==========================================
// SEED DE DATOS EN EL ARRANQUE DE LA APP
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<SubastaContext>();

        // 1. Aplica migraciones pendientes automáticamente
        context.Database.Migrate();

        // 2. Ejecuta el Seeder centralizado
        SeedData.Initialize(context);

        logger.LogInformation("[SEED] Base de datos migrada y sembrada correctamente.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "[SEED-ERROR] Ocurrió un error al migrar o sembrar la base de datos.");
    }
}

app.UseCors("Front");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
