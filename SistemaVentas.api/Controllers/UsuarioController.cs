using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.BLL.Services.Contract;
using SistemaVentas.API.Utility;
using SistemaVentas.DTO;
using SistemaVentas.BLL.Services;
using SistemaVentas.Model;
namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ApiReponseController
    {
        private readonly IUsuarioService _UsuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _UsuarioService = usuarioService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                List<UsuarioDTO> result = await _UsuarioService.List();
                if (result == null || !result.Any())
                {
                    return NotFoundResponse("No se encontraron usuarios.");
                }

                return SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                SesionDTO result = await _UsuarioService.ValidateCredentials(login.Correo, login.Clave);
                if (result == null)
                {
                    return NotFoundResponse("Credenciales inválidas.");
                }
                return SuccessResponse(result);
            }
            catch (Exception ex) { 
                return ErrorResponse(ex);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> create([FromBody] UsuarioDTO usuario)
        {
            try
            {
                UsuarioDTO result = await _UsuarioService.Create(usuario);
                
                return SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> update([FromBody] UsuarioDTO usuario)
        {
            try
            {
                bool result = await _UsuarioService.Update(usuario);

                return SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Delete(int id )
        {
            try
            {
                bool result = await _UsuarioService.Delete(id);

                return SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }
    }
}
