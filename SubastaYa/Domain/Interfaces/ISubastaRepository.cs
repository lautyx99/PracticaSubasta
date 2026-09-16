using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface ISubastaRepository
    {
        Task<Subasta?> GetByIdAsync(int id, CancellationToken cancellationToken );
        Task<List<Subasta>> GetAllAsync( CancellationToken cancellationToken);
        Task<List<Subasta>> GetActivasAsync(CancellationToken cancellationToken);

        Task<List<Subasta>> GetSubastasExpiradasSinFinalizarAsync(CancellationToken cancellationToken = default);

        Task<List<Subasta>> GetByVendedorIdAsync(int vendedorId, CancellationToken cancellationToken);

        Task<List<Subasta>> GetByUsuarioParticipanteAsync(int usuarioId, CancellationToken cancellationToken);

        Task AddAsync(Subasta subasta);
        Task UpdateAsync(Subasta subasta , CancellationToken cancellationToken);
        Task DeleteAsync(Subasta subasta);
    }
}
