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
    public class RolService :IRolService
    {
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol>? rolRepository, IMapper? mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> List()
        {
            try
            {
                List<Rol> rolList = await _rolRepository.Consult().ToListAsync();
                return _mapper.Map<List<RolDTO>>(rolList);
            }
            catch (Exception ex) {
                throw new Exception($"Error al listar roles: {ex.Message}", ex);
            }
        }

    }
}
