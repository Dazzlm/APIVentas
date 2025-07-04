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
    public class VentaController : ApiReponseController
    {
        private readonly IVentaService _ventaService;
        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO model)
        {
            if (model == null)
            {
                return BadRequestResponse("El modelo no puede ser nulo.");
            }
            try
            {
                VentaDTO result = await _ventaService.Register(model);
                return CreatedResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Record(string filter, string? salesNumber,string? startDate, string? endDate)
        {
            try
            {
                salesNumber = salesNumber is null?"":salesNumber;
                startDate = startDate is null ? "" : startDate;
                endDate = endDate is null ? "" : endDate;

                List<VentaDTO> result = await _ventaService.Record(filter,salesNumber, startDate, endDate);

                if (result == null || !result.Any())
                {
                    return NotFoundResponse("No se encontraron ventas.");
                }
                return SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Report( string? startDate, string? endDate)
        {
            try
            {
                startDate = startDate is null ? "" : startDate;
                endDate = endDate is null ? "" : endDate;
                List<ReporteDTO> result = await _ventaService.Report(startDate, endDate);
                if (result == null || !result.Any())
                {
                    return NotFoundResponse("No se encontraron ventas.");
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
