
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.BLL.Services.Contract;
using SistemaVentas.DAL.Repositories.Contract;
using SistemaVentas.DTO;
using SistemaVentas.Model;
namespace SistemaVentas.BLL.Services
{
    public class VentaService: IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepository;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepository, 
            IGenericRepository<DetalleVenta> detalleVentaRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Register(VentaDTO model)
        {
            try
            {
                Venta ventaRegistered = await _ventaRepository.Register(_mapper.Map<Venta>(model));
                if (ventaRegistered == null)
                {
                    throw new Exception("No se pudo registrar la venta");
                }

                return _mapper.Map<VentaDTO>(ventaRegistered);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la venta", ex);
            }
        }
        public async Task<List<VentaDTO>> Record(string filter, string numeroVenta, string numberSales, string startDate, string EndDate)
        {
            IQueryable<Venta> query = _ventaRepository.Consult();
            List<Venta> ventaList = new List<Venta>();

            try
            {
                if (filter == "date") { 
                    DateTime start_Date = DateTime.ParseExact(startDate,"dd/MM/yyyy",new CultureInfo("es-CO"));
                    DateTime end_Date = DateTime.ParseExact(EndDate, "dd/MM/yyyy", new CultureInfo("es-CO"));
                    ventaList = await query.Where(venta => 
                        venta.FechaRegistro.Value.Date >= start_Date.Date &&
                        venta.FechaRegistro.Value.Date <= end_Date
                    ).Include(detalleVenta => detalleVenta.DetalleVenta)
                    .ThenInclude(producto => producto.IdProductoNavigation)
                    .ToListAsync();
                }
                else {
                    ventaList = await query.Where(venta =>
                        venta.NumeroDocumento == numberSales
                    ).Include(detalleVenta => detalleVenta.DetalleVenta)
                    .ThenInclude(producto => producto.IdProductoNavigation)
                    .ToListAsync();

                }

                return _mapper.Map<List<VentaDTO>>(ventaList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el historial de las ventas", ex);
            }
        }

        public async Task<List<ReporteDTO>> Report(string startDate, string EndDate)
        {

            IQueryable<DetalleVenta> query = _detalleVentaRepository.Consult();
            List<DetalleVenta> detalleVentaList = new List<DetalleVenta>();

            try
            {
                DateTime start_Date = DateTime.ParseExact(startDate, "dd/MM/yyyy", new CultureInfo("es-CO"));
                DateTime end_Date = DateTime.ParseExact(EndDate, "dd/MM/yyyy", new CultureInfo("es-CO"));
                detalleVentaList = await query
                .Include(producto => producto.IdProductoNavigation)
                .Include(Venta => Venta.IdVentaNavigation)
                .Where(detalleVenta =>
                    detalleVenta.IdVentaNavigation.FechaRegistro.Value.Date >= start_Date.Date &&
                    detalleVenta.IdVentaNavigation.FechaRegistro.Value.Date <= end_Date
                ).ToListAsync();
                return _mapper.Map<List<ReporteDTO>>(detalleVentaList);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte de las venta", ex);
            }
        }
    }
}
