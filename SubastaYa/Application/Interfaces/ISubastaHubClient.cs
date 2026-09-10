using Application.DTOs.Puja;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ISubastaHubClient
    {
            // Notifica una nueva puja a todos los conectados a la sala
            Task NuevaPujaRecibida(PujaResultadoDto puja);

            // Notifica cuando la regla Anti-Sniping extiende el tiempo de la subasta
            Task TiempoSubastaExtendido(int subastaId, DateTime nuevaFechaFin);

            // Actualiza el estado del temporizador o la finalización de la subasta
            Task EstadoSubastaActualizado(int subastaId, string estado);

            Task SubastaFinalizada(int? ganadorId, decimal? precioFinal);

    }

}
