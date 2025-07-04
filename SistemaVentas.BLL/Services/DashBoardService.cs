using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.BLL.Services.Contract;
using SistemaVentas.DAL.Repositories.Contract;
using SistemaVentas.DTO;
using SistemaVentas.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.AccessControl;
namespace SistemaVentas.BLL.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;

        public DashBoardService(IVentaRepository ventaRepository, IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        public async Task<DashBoardDTO> summary()
        {

            DashBoardDTO vmDashBoard = new DashBoardDTO();

            try {
                vmDashBoard.TotalVentas = await TotalSalesLastWeek();
                vmDashBoard.TotalIngresos = await TotalEarningsLastweek();
                vmDashBoard.TotalProductos = await TotalProducts();

                List<VentasSemanaDTO> salesLastWeek = new List<VentasSemanaDTO>();
                foreach (KeyValuePair<string, int> item in await SalesLastWeek()) {
                    salesLastWeek.Add(new VentasSemanaDTO
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

                vmDashBoard.VentasUtltimaSemana = salesLastWeek;
                return vmDashBoard;
            }
            catch (Exception ex) { 
                throw new Exception($"Error al obtener el resumen del dashboard: {ex.Message}", ex);
            }
        }

        private IQueryable<Venta> ReturnSales(IQueryable<Venta> salesTable, int subtractQuantityDays) { 
            
            DateTime? lastDate = salesTable.OrderByDescending(venta => venta.FechaRegistro)
                .Select(venta => venta.FechaRegistro)
                .First();
            lastDate = lastDate?.AddDays(subtractQuantityDays);
            return salesTable.Where(venta => venta.FechaRegistro >= lastDate.Value.Date);
        }

        private async Task<int> TotalSalesLastWeek()
        {
            IQueryable<Venta> _salesQuery = _ventaRepository.Consult();

            int totalSales = 0;

            if (await _salesQuery.AnyAsync())
            {
                IQueryable<Venta> salesTable = ReturnSales(_salesQuery, -7);
                totalSales = await salesTable.CountAsync();
            }

            return totalSales;
        }

        private async Task<string> TotalEarningsLastweek()
        {
            decimal totalEarnings = 0;
            IQueryable<Venta> _salesQuery = _ventaRepository.Consult();
            if (await _salesQuery.AnyAsync())
            {
                IQueryable<Venta> salesTable = ReturnSales(_salesQuery, -7);
                totalEarnings = await salesTable.Select(venta => venta.Total).SumAsync(venta => venta.Value);
            }
            return Convert.ToString(totalEarnings, new CultureInfo("es-CO"));

        }

        private async Task<int> TotalProducts()
        {
            IQueryable<Producto> _productQuery = _productoRepository.Consult();
            int totalProducts = 0;
            if (await _productQuery.AnyAsync())
            {
                totalProducts = await _productQuery.CountAsync();
            }
            return totalProducts;
        }

        private async Task<Dictionary<string, int>> SalesLastWeek()
        {
            Dictionary<string, int> salesLastWeek = new Dictionary<string, int>();
            IQueryable<Venta> _salesQuery = _ventaRepository.Consult();
            if (await _salesQuery.AnyAsync())
            {
                IQueryable<Venta> salesTable = ReturnSales(_salesQuery, -7);
                salesLastWeek = salesTable
                    .GroupBy(venta => venta.FechaRegistro.Value.Date)
                    .OrderBy(venta => venta.Key)
                    .Select(detalleVenta => new
                    {
                        fecha = detalleVenta.Key.ToString("dd/MM/yyyy"),
                        total = detalleVenta.Count()
                    })
                    .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);
            }

            return salesLastWeek;
        }

    }
}
