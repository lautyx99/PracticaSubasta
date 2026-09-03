using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure
{
    public class Puja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SubastaId { get; set; }

        public int CompradorId { get; set; }

        public decimal Monto { get; set; }

        public DateTime Fecha_Puja { get; set; }

        [ForeignKey("SubastaId")]
        public virtual Subasta Subasta { get; set; }

        [ForeignKey("CompradorId")]
        public virtual Usuario Usuario { get; set; }

    }
}
