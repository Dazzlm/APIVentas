using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DTO;
namespace SistemaVentas.BLL.Services.Contract
{
    public interface IProductoService
    {

        Task<List<ProductoDTO>> List();
        Task<ProductoDTO> Create(ProductoDTO productoDto);
        Task<bool> Update(ProductoDTO productoDto);
        Task<bool> Delete(int id);

    }
}
