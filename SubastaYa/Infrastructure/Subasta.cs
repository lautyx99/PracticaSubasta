using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure
{
    public class Subasta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int VendedorId { get; set; }

        public int CategoriaId { get; set; }

        public int GanadorId { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string UrlImagen { get; set; }

        public decimal PrecioInicial { get; set; }

        public decimal IncrementoMinimo { get; set; }

        public DateTime FechaIncio { get; set; }

        public DateTime FechaFin { get; set; }

        public DateTime FechaFinOriginal { get; set; }

        public string Estado { get; set; }

        public int version { get; set; }

        [ForeignKey("VendedorId")]
        public virtual Usuario Vendedor { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria Cateogria { get; set; }
    }
}
