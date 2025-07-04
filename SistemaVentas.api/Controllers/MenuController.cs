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
    public class MenuController : ApiReponseController
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int idUsuario) {
            try
            {
                List<MenuDTO> result = await _menuService.List(idUsuario);
                if (result == null || !result.Any())
                {
                    return NotFoundResponse("No se encontraron Menus.");
                }
                return SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }

        }
    }
}
