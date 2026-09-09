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
            catch (DbUpdateConcurrencyException )
            {
                // Manejo de Optimistic Locking -> 409 Conflict
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
            catch (InvalidOperationException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var problem = new { Status = 400, Title = "Regla de Negocio Violada", Detail = ex.Message };
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
            catch (KeyNotFoundException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var problem = new { Status = 404, Title = "Recurso No Encontrado", Detail = ex.Message };
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = new { Status = 500, Title = "Error Interno", Detail = $"[CODE-ERROR] - {ex.Message}" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}

