using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Escanea todas las clases concretas en Application que sean Casos de Uso
            var useCases = assembly.GetTypes()
                .Where(t => t.IsClass
                         && !t.IsAbstract
                         && (t.Name.EndsWith("UseCase") || t.Name.StartsWith("Realizar") || t.Name.StartsWith("Obtener")));

            foreach (var useCase in useCases)
            {
                services.AddScoped(useCase);
            }

            return services;
        }
    }
}
