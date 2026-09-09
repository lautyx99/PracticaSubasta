using Application.DTOs.Categoria;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Categorias
{
    public class ObtenerCategoriaPorId
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ObtenerCategoriaPorId(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<CategoriaDto?> ExecuteAsync(int categoriaId)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(categoriaId);

            if (categoria == null)
            {
                return null;
            }
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                UrlIcono = categoria.UrlIcono
            };
        }

    }
}
