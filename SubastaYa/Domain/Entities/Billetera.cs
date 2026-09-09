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

        public int Version { get; private set; }

        public decimal SaldoDisponible => SaldoTotal - SaldoRetenido;

        public virtual Usuario Usuario { get; private set; } = null!;

        public virtual ICollection<TransaccionLedger> Transacciones { get; private set; } = new List<TransaccionLedger>();

        private Billetera() { }

        public Billetera(int usuarioId, decimal saldoTotal, decimal saldoRetenido, int version)
        {
            UsuarioId = usuarioId;
            SaldoTotal = saldoTotal;
            SaldoRetenido = saldoRetenido;
            Version = version;
        }

        // Método para realizar el depósito de forma segura
        public void Depositar(decimal monto)
        {
            if (monto <= 0)
            {
                throw new ArgumentException("[CODE-ERROR] - El monto a depositar debe ser mayor a cero.", nameof(monto));
            }

            // Al incrementar el SaldoTotal, el SaldoDisponible aumenta automáticamente
            SaldoTotal += monto;

            // Control de concurrencia optimista
            Version++;
        }

        public void RetenerFondos(decimal monto)
        {
            if (monto <= 0)
                throw new ArgumentException("[CODE-ERROR] - El monto a retener debe ser mayor a cero.", nameof(monto));

            if (SaldoDisponible < monto)
                throw new InvalidOperationException("[CODE-ERROR] - Saldo disponible insuficiente para realizar la retención.");

            SaldoRetenido += monto;
            Version++;
        }

        public void LiberarFondos(decimal monto)
        {
            if (monto <= 0)
                throw new ArgumentException("[CODE-ERROR] - El monto a liberar debe ser mayor a cero.", nameof(monto));

            if (SaldoRetenido < monto)
                throw new InvalidOperationException("[CODE-ERROR] - Inconsistencia contable: Intentando liberar mas saldo del retenido.");

            SaldoRetenido -= monto;
            Version++;
        }
    }
}
