using Application.UseCases.Finalizacion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Workers
{
    public class SubastaWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubastaWorker> _logger;
        private readonly TimeSpan _periodoInspeccion = TimeSpan.FromSeconds(5); 

        public SubastaWorker(
            IServiceProvider serviceProvider,
            ILogger<SubastaWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[WORKER] SubastaWorker iniciado y monitoreando subastas activas.");

            // Pequeña pausa inicial para dar tiempo a que la API levante y corran las migraciones/seeds
            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);

            using var timer = new PeriodicTimer(_periodoInspeccion);

            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<SubastaContext>();

                    // 🛡️ VALIDACIÓN DE SEGURIDAD: Si la tabla de usuarios aún está vacía, 
                    // el worker se espera al siguiente ciclo para evitar el error de FK.
                    if (!context.Usuarios.Any())
                    {
                        _logger.LogWarning("[WORKER] Esperando a que el seed de usuarios esté listo...");
                        continue;
                    }

                    var finalizarSubastasUseCase = scope.ServiceProvider.GetRequiredService<FinalizarSubastasExpiradas>();
                    await finalizarSubastasUseCase.ExecuteAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[WORKER-ERROR] Error al procesar el cierre automático de subastas.");
                }
            }
        }
    }
}
