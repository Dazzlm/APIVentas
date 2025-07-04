using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVentas.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DAL.Repositories.Contract;
using SistemaVentas.DAL.Repositories;
using SistemaVentas.Utility;
using SistemaVentas.BLL.Services;
using SistemaVentas.BLL.Services.Contract;

namespace SistemaVentas.IOC
{
    public static class Dependence
    {
        public static void InjectDependencies(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DbVentaContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>(); 
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<IMenuService, MenuService>();

        }
    }
}
