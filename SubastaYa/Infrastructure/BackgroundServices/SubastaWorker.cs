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
        private readonly TimeSpan _periodoInspeccion = TimeSpan.FromSeconds(5); // Frecuencia del ciclo

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

            using var timer = new PeriodicTimer(_periodoInspeccion);

            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Crear un scope manual para resolver servicios Scoped (DbContext / UseCases)
                    using var scope = _serviceProvider.CreateScope();
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
