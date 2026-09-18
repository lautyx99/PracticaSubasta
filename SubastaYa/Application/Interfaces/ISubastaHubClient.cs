using Application.DTOs.Puja;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ISubastaHubClient
    {
            Task NuevaPujaRecibida(PujaResultadoDto puja);

            Task TiempoSubastaExtendido(int subastaId, DateTime nuevaFechaFin);

            Task EstadoSubastaActualizado(int subastaId, string estado);

            Task SubastaFinalizada(int? ganadorId, decimal? precioFinal);

    }

}
