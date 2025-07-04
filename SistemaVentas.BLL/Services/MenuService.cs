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
    public class MenuService :IMenuService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IGenericRepository<MenuRol> _menuRolRepository;
        private readonly IGenericRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Usuario> usuarioRepository, IGenericRepository<MenuRol> menuRolRepository, IGenericRepository<Menu> menuRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _menuRolRepository = menuRolRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> List(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = _usuarioRepository.Consult(usuario =>usuario.IdUsuario == idUsuario);
            IQueryable<MenuRol> tbMenuRol = _menuRolRepository.Consult();
            IQueryable<Menu> tbMenu = _menuRepository.Consult();

            try
            {
                IQueryable<Menu> tbResult = (from u in tbUsuario
                                            join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                            join m in tbMenu on mr.IdMenu equals m.IdMenu
                                            select m).AsQueryable();
                List<Menu> menuList = await tbResult.ToListAsync();
                return _mapper.Map<List<MenuDTO>>(menuList);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar los menús: {ex.Message}", ex);
            }
        }
    }
}
