using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Billetera
{
    public class MovimientoDto
    {
        public int Id { get; set; }
        public int BilleteraId { get; set; }

        public int UsuarioId { get; set; }

        public decimal Monto { get; set; }
        public string Tipo { get; set; } = string.Empty; // "Deposito", "Retiro", "Puja", etc.
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
    }
}
