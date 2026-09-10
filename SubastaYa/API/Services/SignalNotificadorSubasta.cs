using API.Hubs;
using Application.DTOs.Puja;
using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.Services
{
    public class SignalNotificadorSubasta : INotificadorSubasta
    {
        private readonly IHubContext<SubastaHub, ISubastaHubClient> _hubContext;

        public SignalNotificadorSubasta(IHubContext<SubastaHub, ISubastaHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotificarNuevaPujaAsync(int subastaId, PujaResultadoDto puja)
        {
            string grupo = $"Subasta_{subastaId}";
            await _hubContext.Clients.Group(grupo).NuevaPujaRecibida(puja);
        }

        public async Task NotificarTiempoExtendidoAsync(int subastaId, DateTime nuevaFechaFin)
        {
            string grupo = $"Subasta_{subastaId}";
            await _hubContext.Clients.Group(grupo).TiempoSubastaExtendido(subastaId, nuevaFechaFin);
        }

        public async Task NotificarSubastaFinalizadaAsync(int subastaId, int? ganadorId, decimal? precioFinal)
        {
            await _hubContext.Clients
                .Group($"Subasta_{subastaId}")
                .SubastaFinalizada(ganadorId, precioFinal);
        }
    }
}
