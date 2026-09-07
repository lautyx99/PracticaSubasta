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

        private Billetera() { }

        public Billetera(int usuarioId, decimal saldoTotal, decimal saldoRetenido, decimal saldoDisponible, int version)
        {
            UsuarioId = usuarioId;
            SaldoTotal = saldoTotal;
            SaldoRetenido = saldoRetenido;
            SaldoDisponible = saldoDisponible;
            Version = version;
        }
    }
}
