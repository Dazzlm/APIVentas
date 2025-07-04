using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DTO;
namespace SistemaVentas.BLL.Services.Contract
{
    public interface IVentaService
    {
        Task<VentaDTO> Register(VentaDTO model);
        Task<List<VentaDTO>> Record(string filter, string numberSales, string startDate, string EndDate);
        Task<List<ReporteDTO>> Report(string startDate, string EndDate);
    }
}
