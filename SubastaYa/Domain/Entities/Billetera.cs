using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Billetera
    {
        public int Id { get; private set; }

        public int UsuarioId { get; private set; }

        public decimal SaldoTotal { get; private set; }

        public decimal SaldoRetenido { get; private set; }

        public decimal SaldoDisponible { get; private set; }

        public int Version { get; private set; }
        public virtual Usuario Usuario { get; private set; } = null!;

        public virtual ICollection<TransaccionLedger> Transacciones { get; private set; } = new List<TransaccionLedger>();

        private Billetera() { }

        public Billetera(int usuarioId, decimal saldoTotal, decimal saldoRetenido, decimal saldoDisponible, int version)
        {
            UsuarioId = usuarioId;
            SaldoTotal = saldoTotal;
            SaldoRetenido = saldoRetenido;
            SaldoDisponible = saldoDisponible;
            Version = version;
        }

        // Método para realizar el depósito de forma segura
        public void Depositar(decimal monto)
        {
            if (monto <= 0)
            {
                throw new ArgumentException("El monto a depositar debe ser mayor a cero.", nameof(monto));
            }

            // Reglas financieras
            SaldoTotal += monto;
            SaldoDisponible += monto;

            // Control de concurrencia optimista (si lo manejas manualmente)
            Version++;
        }
    }
}
