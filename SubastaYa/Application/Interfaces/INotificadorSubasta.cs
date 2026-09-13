using Application.DTOs.Puja;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface INotificadorSubasta
    {
        Task NotificarNuevaPujaAsync(int subastaId, PujaResultadoDto puja);

        Task NotificarTiempoExtendidoAsync(int subastaId, DateTime nuevaFechaFin);

        Task NotificarSubastaFinalizadaAsync(int subastaId, int? ganadorId, decimal? precioFinal);
    }
}
