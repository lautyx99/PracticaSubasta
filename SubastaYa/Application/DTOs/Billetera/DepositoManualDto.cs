using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Billetera
{
    public class DepositoManualDto
    {
        public int UsuarioAfectadoId { get; set; }
        public decimal Monto { get; set; }
        public string Motivo { get; set; } = null!;
    }
}
