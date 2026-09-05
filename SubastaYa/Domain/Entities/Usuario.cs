using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; private set; }

        public string Email { get; private set; } 

        public string Nombre { get; private set; }

        public string ContraseñaHash { get; private set; }

        public DateTime FechaRegistro { get; private set; }

        public string Rol { get; private set; }

        private Usuario() { }

        public Usuario(string email, string nombre, string contraseñaHash, DateTime fechaRegistro, string rol)
        {
            Email = email;
            Nombre = nombre;
            ContraseñaHash = contraseñaHash;
            FechaRegistro = fechaRegistro;
            Rol = rol;
        }
    }
}
