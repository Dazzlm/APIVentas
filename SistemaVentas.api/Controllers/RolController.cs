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
    public class RolController : ApiReponseController
    {

        private readonly IRolService _rolService;
        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            
            try
            {
                List<RolDTO> result = await _rolService.List();
                if (result == null || !result.Any())
                {
                    return NotFoundResponse("No se encontraron roles.");
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
