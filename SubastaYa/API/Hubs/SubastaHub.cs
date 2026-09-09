using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class SubastaHub : Hub<ISubastaHubClient>
    {
        // El cliente (frontend) llama a este método para unirse a la sala de una subasta específica
        public async Task UnirseASalaSubasta(int subastaId)
        {
            string grupoNombre = GetGrupoNombre(subastaId);
            await Groups.AddToGroupAsync(Context.ConnectionId, grupoNombre);
        }

        // El cliente llama a este método al salir de la vista de la subasta
        public async Task SalirDeSalaSubasta(int subastaId)
        {
            string grupoNombre = GetGrupoNombre(subastaId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, grupoNombre);
        }

        private static string GetGrupoNombre(int subastaId) => $"Subasta_{subastaId}";
    }
}
