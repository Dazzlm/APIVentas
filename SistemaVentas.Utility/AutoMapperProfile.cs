using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVentas.Model;
using SistemaVentas.DTO;
using System.Globalization;

namespace SistemaVentas.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region ROl
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destination =>
                    destination.RolDescripcion,
                    options => options.MapFrom(origen => origen.IdRolNavigation.Nombre)
                ).ForMember(destination =>
                    destination.EsActivo,
                    options => options.MapFrom(origen => origen.EsActivo == true ?  1:0)
                );

            CreateMap<Usuario, SesionDTO>()
                .ForMember(destination =>
                    destination.RolDescripcion,
                    options => options.MapFrom(origen => origen.IdRolNavigation.Nombre)
                );
            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destination =>
                    destination.IdRolNavigation,
                    options => options.Ignore()
                ).ForMember(destination =>
                    destination.EsActivo,
                    options => options.MapFrom(origen => origen.EsActivo == 1 ? true : false)
                );
            #endregion

            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion

            #region Producto
            CreateMap<Producto, ProductoDTO>()
                .ForMember(destination =>
                    destination.DescripcionCategoria,
                    options => options.MapFrom(origen => origen.IdCategoriaNavigation.Nombre)
                )
                .ForMember(destination =>
                    destination.Precio,
                    options => options.MapFrom(origen => Convert.ToString(origen.Precio.Value,new CultureInfo("es-CO")))
                ).ForMember(destination =>
                    destination.EsActivo,
                    options => options.MapFrom(origen => origen.EsActivo == true ? 1 : 0)
                );
            CreateMap<ProductoDTO, Producto>()
                .ForMember(destination =>
                    destination.IdCategoriaNavigation,
                    options => options.Ignore()
                )
                .ForMember(destination =>
                    destination.Precio,
                    options => options.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-CO")))
                ).ForMember(destination =>
                    destination.EsActivo,
                    options => options.MapFrom(origen => origen.EsActivo == 1 ? true : false)
                );
            #endregion

            #region Venta
            CreateMap<Venta, VentaDTO>()
                .ForMember(destination =>
                    destination.TotalTexto,
                    options => options.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-CO")))
                )
                .ForMember(destination =>
                    destination.FechaRegistro,
                    options => options.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                );

            CreateMap<VentaDTO, Venta>()
                .ForMember(destination =>
                    destination.Total,
                    options => options.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-CO")))
                );
            #endregion

            #region DetalleVenta
            CreateMap<DetalleVenta, DetalleVentaDTO>()
                .ForMember(destination => 
                    destination.DescripcionProducto,
                    options => options.MapFrom(origen => origen.IdProductoNavigation.Nombre)
                ).ForMember(destination =>
                    destination.PrecioTexto,
                    options => options.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-CO")))
                ).ForMember(destination =>
                    destination.TotalTexto,
                    options => options.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-CO")))
                );

            CreateMap<DetalleVentaDTO, DetalleVenta>()
                .ForMember(destination =>
                    destination.Precio,
                    options => options.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-CO")))
                ).ForMember(destination =>
                    destination.Total,
                    options => options.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-CO")))
                );

            #endregion

            #region Reporte
            CreateMap<DetalleVenta, ReporteDTO>()
                .ForMember(destination =>
                    destination.FechaRegistro,
                    options => options.MapFrom(origen => origen.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                )
                .ForMember(destination =>
                    destination.NumeroDocumento,
                    options => options.MapFrom(origen => origen.IdVentaNavigation.NumeroDocumento)
                )
                .ForMember(destination =>
                    destination.TipoPago,
                    options => options.MapFrom(origen => origen.IdVentaNavigation.TipoPago)
                ).ForMember(destination =>
                    destination.TotalVenta,
                    options => options.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-CO")))
                ).ForMember(destination =>
                    destination.Producto,
                    options => options.MapFrom(origen => origen.IdProductoNavigation.Nombre)
                ).ForMember(destination =>
                    destination.Precio,
                    options => options.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-CO")))
                ).ForMember(destination =>
                    destination.Total,
                    options => options.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-CO")))
                );
            #endregion

        }
    }
}
