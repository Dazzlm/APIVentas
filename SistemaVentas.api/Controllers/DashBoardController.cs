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
    public class DashBoardController : ApiReponseController
    {
        private readonly IDashBoardService _dashBoardService;
        public DashBoardController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }

        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Summary()
        {
            try
            {
                DashBoardDTO result = await _dashBoardService.Summary();
                if (result == null)
                {
                    return NotFoundResponse("No se encontraron datos de resumen.");
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
