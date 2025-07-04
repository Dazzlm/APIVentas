using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.BLL.Services.Contract;
using SistemaVentas.API.Utility;
using SistemaVentas.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ApiReponseController
    {

        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                List<CategoriaDTO> result = await _categoriaService.List();
                if (result == null || !result.Any())
                {
                    return NotFoundResponse("No se encontraron categorías.");
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
