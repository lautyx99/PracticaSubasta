using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class CorporateApiMiddleware
    {
        private readonly RequestDelegate _next;

        public CorporateApiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Estándar 3: Inyección global obligatoria del header X-Api-version
            context.Response.Headers.Append("X-Api-version", "1.0");

            try
            {
                await _next(context);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;

                    var problem = new
                    {
                        Status = 409,
                        Title = "Conflicto de Concurrencia",
                        Detail = "[CODE-ERROR] - La subasta o la billetera fue modificada por otra transaccion simultanea. Por favor, reintente."
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
                }
            }
            catch (ArgumentException ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    var problem = new
                    {
                        Status = 400,
                        Title = "Parámetro Inválido",
                        Detail = ex.Message
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
                }
            }
            catch (InvalidOperationException ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    var problem = new
                    {
                        Status = 400,
                        Title = "Regla de Negocio Violada",
                        Detail = ex.Message
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
                }
            }
            catch (KeyNotFoundException ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                    var problem = new { Status = 404, Title = "Recurso No Encontrado", Detail = ex.Message };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
                }
            }
            catch (Exception ex)
            {
                // LOG TEMPORAL PARA AUDITAR LA INNER EXCEPTION DE ENTITY FRAMEWORK
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"[EF CORE ERROR]: {innerMessage}");

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var problem = new { Status = 500, Title = "Error Interno", Detail = $"[CODE-ERROR] - {innerMessage}" };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
                }
            }
        }
    }
}

