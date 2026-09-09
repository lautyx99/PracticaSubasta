using Application.DTOs.Categoria;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Categorias
{
    public class ObtenerCategoria
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ObtenerCategoria(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaDto>> ExecuteAsync()
        {
            var categoria = await _categoriaRepository.GetAllAsync();

            if (categoria == null)
            {
                return Enumerable.Empty<CategoriaDto>();
            }

            return categoria.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                UrlIcono = c.UrlIcono
            });
        }


    }
}
