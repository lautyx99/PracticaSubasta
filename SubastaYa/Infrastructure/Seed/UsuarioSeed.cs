using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class UsuarioSeed 
    {
        public static void Seed(SubastaContext context)
        {
            if (context.Usuarios.Any())
            {
                return; // Los usuarios ya han sido sembrados
            }

            var fecha = DateTime.UtcNow;

            // Password por defecto hasheada para todos los usuarios de prueba
            string contraseñaHash = BCrypt.Net.BCrypt.HashPassword("123456");

            var usuarios = new List<Usuario>
            {
                new Usuario(
                    nombre: "Vendedor Test",
                    email: "vendedor@test.com",
                    contraseñaHash: contraseñaHash,
                    rol: RolUsuario.Vendedor,
                    fechaRegistro: fecha
                ),

                new Usuario(
                    nombre: "Comprador Uno",
                    email: "comprador1@test.com",
                    contraseñaHash: contraseñaHash,
                    rol: RolUsuario.Comprador,
                    fechaRegistro: fecha
                ),

                new Usuario(
                    nombre: "Comprador Dos",
                    email: "comprador2@test.com",
                    contraseñaHash: contraseñaHash,
                    rol: RolUsuario.Comprador,
                    fechaRegistro: fecha
                ),

                new Usuario(
                    nombre: "Sinfondos",
                    email: "sinfondos@test.com",
                    contraseñaHash: contraseñaHash,
                    rol: RolUsuario.Comprador,
                    fechaRegistro: fecha
                ),
                
                new Usuario(
                    nombre: "Administrador",
                    email : "admin@test.com",
                    contraseñaHash : contraseñaHash,
                    rol: RolUsuario.Administrador,
                    fechaRegistro : fecha
                    )
            };

            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();
        }
    }
}
