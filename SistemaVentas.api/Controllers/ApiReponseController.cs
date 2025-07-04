using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SistemaVentas.API.Utility;
namespace SistemaVentas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiReponseController : ControllerBase
    {
        protected IActionResult SuccessResponse<T>(T data)
        {
            return Ok(new Response<T>
            {
                Status = true,
                Value = data
            });
        }

        protected IActionResult CreatedResponse<T>(T? data = default)
        {
            return CreatedAtAction(nameof(SuccessResponse), new Response<T>
            {
                Status = true,
                Value = data
            });
        }

        protected IActionResult BadRequestResponse(string message = "Solicitud incorrecta o datos inválidos", ICollection<ModelStateEntry>? modelStateEntries = null )
        {
            List<string>? errors = GetModelErrors(modelStateEntries); 

            return BadRequest(new Response<object>
            {
                Status = false,
                Message = message,
                Value = errors
            });
        }


        protected IActionResult NotFoundResponse(string message = "Recurso no encontrado o no hay elementos a mostrar")
        {
            return NotFound(new Response<string>
            {
                Status = false,
                Message = message
            });
        }

        protected IActionResult ErrorResponse(Exception ex)
        {
            return StatusCode(500, new Response<string>
            {
                Status = false,
                Message = $"Error interno: {ex.Message}"
            });
        }

        protected IActionResult NoContentResponse()
        {
            return NoContent();
        }

        protected List<string>? GetModelErrors(ICollection<ModelStateEntry> modelStateEntries)
        {
            if (modelStateEntries == null )
            {
                return null ;
            }

            return modelStateEntries
                             .SelectMany(v => v.Errors)
                             .Select(e => e.ErrorMessage)
                             .ToList();
        }
    }
}
