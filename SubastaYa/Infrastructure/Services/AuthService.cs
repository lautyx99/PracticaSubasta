using Application.DTOs.Auth;
using Application.DTOs.Login;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.ContraseñaHash))
            {
                throw new InvalidOperationException("Credenciales inválidas.");
            }

            string token = GenerarJwtToken(usuario);
            return new AuthResponseDto(usuario.Id, usuario.Nombre, usuario.Email, token);
        }

        public async Task<AuthResponseDto> RegistrarAsync(RegistroRequestDto request)
        {
            var usuarioExiste = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (usuarioExiste != null)
            {
                throw new InvalidOperationException("El correo ya está registrado.");
            }
            var fechaRegistro = DateTime.UtcNow;
            var rolPorDefecto = RolUsuario.Comprador;


            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var nuevoUsuario = new Usuario(request.Nombre, request.Email, passwordHash,fechaRegistro, rolPorDefecto );

            await _usuarioRepository.AddAsync(nuevoUsuario);

            string token = GenerarJwtToken(nuevoUsuario);
            return new AuthResponseDto(nuevoUsuario.Id, nuevoUsuario.Nombre, nuevoUsuario.Email, token);
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nombre)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiracionEnMinutos"] ?? "120")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
