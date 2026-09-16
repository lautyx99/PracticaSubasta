using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTOs.Auth
{
      public record AuthResponseDto(int Id, string Nombre, string Email, string Token, [property: JsonConverter(typeof(JsonStringEnumConverter))] RolUsuario Rol);

}
