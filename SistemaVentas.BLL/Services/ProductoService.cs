using System;
using System.Collections.Generic;
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
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        public  async Task<List<ProductoDTO>> List()
        {
            try
            {
                IQueryable<Producto> productQuery = _productoRepository.Consult();
                List<Producto> productList = await productQuery.Include(categoria =>
                    categoria.IdCategoriaNavigation
                    ).ToListAsync();
                return _mapper.Map<List<ProductoDTO>>(productList);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar productos: {ex.Message}", ex);
            }
        }
        public async Task<ProductoDTO> Create(ProductoDTO productoDto)
        {
            try
            {
                Producto createProduct = await _productoRepository.Create(_mapper.Map<Producto>(productoDto));

                if (createProduct == null)
                {
                    throw new Exception("No se pudo crear el producto.");
                }

                return _mapper.Map<ProductoDTO>(createProduct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el producto: {ex.Message}", ex);
            }
        }

        public async Task<bool> Update(ProductoDTO productoDto)
        {
            try
            {
                Producto productModel = _mapper.Map<Producto>(productoDto);
                Producto? productFound = await _productoRepository.Get(p => p.IdProducto == productModel.IdProducto);

                if (productFound == null)
                {
                    throw new Exception("Producto no encontrado.");
                }

                productFound.Nombre = productModel.Nombre;
                productFound.IdCategoria = productModel.IdCategoria;
                productFound.Stock = productModel.Stock;
                productFound.Precio = productModel.Precio;
                productFound.EsActivo = productModel.EsActivo;

                bool response = await _productoRepository.Update(productFound);
                if (!response)
                {
                    throw new Exception("No se pudo actualizar el producto.");
                }
                return response;


            }
            catch (Exception ex)
            {
                throw new Exception($"Error al editar el producto: {ex.Message}", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                Producto? productFound = await _productoRepository.Get(p => p.IdProducto == id);

                if (productFound == null)
                {
                    throw new Exception("Producto no encontrado.");
                }

                bool response = await _productoRepository.Delete(productFound);

                if (!response)
                {
                    throw new Exception("No se pudo eliminar el producto.");
                }
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el producto: {ex.Message}", ex);
            }
        }

    }
}
