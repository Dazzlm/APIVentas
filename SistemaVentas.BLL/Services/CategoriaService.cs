using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.BLL.Services.Contract;
using SistemaVentas.DAL.Repositories.Contract;
using SistemaVentas.DTO;
using SistemaVentas.Model;

namespace SistemaVentas.BLL.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> List()
        {
            try
            {
                List<Categoria> categoriaList = await _categoriaRepository.Consult().ToListAsync();
                return  _mapper.Map<List<CategoriaDTO>>(categoriaList);
            }
            catch (Exception ex) { 
                throw new Exception($"Error al listar categorias: {ex.Message}", ex);
            }
        }
    }
}
