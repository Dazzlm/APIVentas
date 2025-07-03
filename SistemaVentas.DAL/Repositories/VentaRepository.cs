using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DAL.Repositories.Contract;
using SistemaVentas.DAL.DBContext;
using SistemaVentas.Model;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaVentas.DAL.Repositories
{
    public class VentaRepository :GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbVentaContext _dbcontext;

        public VentaRepository(DbVentaContext dbcontext):base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> Register(Venta model)
        {
            Venta salesGenerated = new Venta();

            using (var transaction = _dbcontext.Database.BeginTransaction()) {
                try
                {
                    foreach (DetalleVenta dv in model.DetalleVenta) { 
                        Producto product_found = _dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        product_found.Stock -= dv.Cantidad;
                        _dbcontext.Productos.Update(product_found);
                    }
                    await _dbcontext.SaveChangesAsync();

                    NumeroDocumento correlative = _dbcontext.NumeroDocumentos.First();
                    correlative.UltimoNumero += 1;
                    correlative.FechaRegistro = DateTime.Now;
                    _dbcontext.NumeroDocumentos.Update(correlative);
                    await _dbcontext.SaveChangesAsync();

                    int cantDigitos = 4;
                    string zeros = string.Concat(Enumerable.Repeat("0", cantDigitos));
                    string saleNumber = zeros +correlative.UltimoNumero.ToString();

                    saleNumber = saleNumber.Substring(saleNumber.Length - cantDigitos, cantDigitos);
                    model.NumeroDocumento = saleNumber;
                    await _dbcontext.Venta.AddAsync(model);
                    await _dbcontext.SaveChangesAsync();

                    salesGenerated = model;
                    transaction.Commit();

                }
                catch (Exception ex) {

                    transaction.Rollback();
                    throw new Exception("Error al registrar la venta "+ex);
                }
                return salesGenerated;

            }
        }
    }
}
