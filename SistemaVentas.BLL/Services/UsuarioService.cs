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
using static System.Net.Mime.MediaTypeNames;

namespace SistemaVentas.BLL.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario>? rolRepository, IMapper? mapper)
        {
            _usuarioRepository = rolRepository;
            _mapper = mapper;
        }
        public async Task<List<UsuarioDTO>> List()
        {
            try
            {
                IQueryable<Usuario> queryUsuario = _usuarioRepository.Consult();
                List<UsuarioDTO> usuarioList = _mapper.Map<List<UsuarioDTO>>(await queryUsuario.ToListAsync());
                return usuarioList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar usuarios: {ex.Message}", ex);
            }
        }
        public async Task<SesionDTO> ValidateCredentials(string email, string password)
        {
            try
            {
                IQueryable<Usuario> queryUsuario = _usuarioRepository.Consult(usuario =>
                    usuario.Correo == email &&
                    usuario.Clave == password
                );

                if(await queryUsuario.FirstOrDefaultAsync() == null)
                {
                    throw new TaskCanceledException("Usuario o contraseña incorrectos.");
                }

                Usuario user = await queryUsuario.Include(rol => rol.IdRolNavigation).FirstAsync();
                return _mapper.Map<SesionDTO>(user);
            }
            catch(Exception ex) {
                throw new Exception($"Error al validar credenciales: {ex.Message}", ex);
            }
        }

        public async Task<UsuarioDTO> Create(UsuarioDTO model)
        {
            try
            {
                Usuario createdUser = await _usuarioRepository.Create(_mapper.Map<Usuario>(model));
                if (createdUser == null)
                {
                    throw new TaskCanceledException("No se puedo crear el usuario");
                }

                IQueryable<Usuario> queryUsuario = _usuarioRepository.Consult(usuario => usuario.IdUsuario == createdUser.IdUsuario);
                createdUser = await queryUsuario.Include(rol => rol.IdRolNavigation).FirstAsync();
                return _mapper.Map<UsuarioDTO>(createdUser);
            }
            catch (Exception ex)
            { 
                throw new Exception($"Error al crear usuario: {ex.Message}", ex);
            }
        }

        public async Task<bool> Update(UsuarioDTO model)
        {
            try
            {
                Usuario modelUser = _mapper.Map<Usuario>(model);
                Usuario? userFound = await _usuarioRepository.Get(usuario =>
                    usuario.IdUsuario == modelUser.IdUsuario
                );

                if (userFound == null)
                {
                    throw new TaskCanceledException("Usuario no encontrado");
                }

                userFound.NombreCompleto = modelUser.NombreCompleto;
                userFound.Correo = modelUser.Correo;
                userFound.IdRol = modelUser.IdRol;
                userFound.Clave = modelUser.Clave;
                userFound.EsActivo = modelUser.EsActivo;
                userFound.FechaRegistro = DateTime.Now;

                bool response = await _usuarioRepository.Update(userFound);
                if (!response)
                {
                    throw new TaskCanceledException("No se pudo actualizar el usuario");
                }
                return response;

            }
            catch(Exception ex) {
                throw new Exception($"Error al actualizar usuario: {ex.Message}", ex);
            }
        }

        public async Task<bool> Delete(int idUsuario)
        {
            try
            {
                Usuario? userFound = await _usuarioRepository.Get(usuario =>
                    usuario.IdUsuario == idUsuario
                );

                if (userFound == null)
                {
                    throw new TaskCanceledException("Usuario no encontrado");
                }

                bool response = await _usuarioRepository.Delete(userFound);
                if (!response)
                {
                    throw new TaskCanceledException("No se pudo eliminar el usuario");
                }

                return response;

            }
            catch (Exception ex) { 
                throw new Exception($"Error al eliminar usuario: {ex.Message}", ex);
            }
        }

    }
}
