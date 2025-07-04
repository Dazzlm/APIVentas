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
    public class ProductoController : ApiReponseController
    {
        private readonly IProductoService _productoService;
        
        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                List<ProductoDTO> result = await _productoService.List();
                if (result == null || !result.Any())
                {
                    return NotFoundResponse("No se encontraron productos.");
                }
                return SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Create([FromBody] ProductoDTO model)
        {
            if (model == null)
            {
                return BadRequestResponse("El modelo no puede ser nulo.");
            }
            try
            {
                ProductoDTO result = await _productoService.Create(model);
                return CreatedResponse(result);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Update([FromBody] ProductoDTO model)
        {
            if (model == null)
            {
                return BadRequestResponse("El modelo no puede ser nulo.");
            }
            try
            {
                bool result = await _productoService.Update(model);
                if (!result)
                {
                    return NotFoundResponse("Producto no encontrado o no se pudo actualizar.");
                }
                return NoContentResponse();
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _productoService.Delete(id);
                if (!result)
                {
                    return NotFoundResponse("Producto no encontrado o no se pudo eliminar.");
                }
                return NoContentResponse();
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }

    }

}
