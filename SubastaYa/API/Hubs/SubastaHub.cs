using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    [Authorize]
    public class SubastaHub : Hub<ISubastaHubClient>
    {
        private readonly ILogger<SubastaHub> _logger;

        public SubastaHub(ILogger<SubastaHub> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// El cliente (frontend) llama a este método para unirse a la sala en tiempo real de una subasta específica.
        /// </summary>
        public async Task UnirseASalaSubasta(int subastaId)
        {
            string grupoNombre = GetGrupoNombre(subastaId);
            string usuarioId = Context.UserIdentifier ?? "Anonimo";

            await Groups.AddToGroupAsync(Context.ConnectionId, grupoNombre);

            _logger.LogInformation(
                "[SIGNALR] Usuario {UsuarioId} (ConnectionId: {ConnectionId}) se unió al grupo {GrupoNombre}",
                usuarioId, Context.ConnectionId, grupoNombre);
        }

        /// <summary>
        /// El cliente llama a este método al salir de la pantalla/vista de la subasta.
        /// </summary>
        public async Task SalirDeSalaSubasta(int subastaId)
        {
            string grupoNombre = GetGrupoNombre(subastaId);
            string usuarioId = Context.UserIdentifier ?? "Anonimo";

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, grupoNombre);

            _logger.LogInformation(
                "[SIGNALR] Usuario {UsuarioId} salió del grupo {GrupoNombre}",
                usuarioId, grupoNombre);
        }

        /// <summary>
        /// Maneja la desconexión automática del WebSocket.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation(
                "[SIGNALR] Conexión {ConnectionId} finalizada.",
                Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        private static string GetGrupoNombre(int subastaId) => $"Subasta_{subastaId}";
    }

}
