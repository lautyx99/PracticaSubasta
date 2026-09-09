using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IBilleteraRepository
    {
        Task<Billetera?> GetByUsuarioIdAsync(int usuarioId);

        Task<Billetera?> GetByIdWithTransaccionesAsync(int billeteraId);

        Task UpdateAsync(Billetera billetera);
    }
}
