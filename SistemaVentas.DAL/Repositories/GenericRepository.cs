using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DAL.Repositories.Contract;
using SistemaVentas.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace SistemaVentas.DAL.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly DbVentaContext _dbcontext;

        public GenericRepository(DbVentaContext context)
        {
            _dbcontext = context;
        }

        public async Task<TModel?> Get(Expression<Func<TModel, bool>> filter)
        {
            try
            {
                TModel? model = await _dbcontext.Set<TModel>().FirstOrDefaultAsync(filter);
                return model;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener el registro de {typeof(TModel).Name}", ex);
            }

        }

        public IQueryable<TModel> Consult(Expression<Func<TModel, bool>>? filter = null)
        {
            try
            {
                IQueryable<TModel> queryModel = filter == null ? _dbcontext.Set<TModel>() : _dbcontext.Set<TModel>().Where(filter);
                return queryModel;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al general el query en {typeof(TModel).Name}", ex);
            }
        }

        public async Task<TModel> Create(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Add(model);
                await _dbcontext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al crear el registro de {typeof(TModel).Name}", ex);
            }
        }

        public async Task<bool> Update(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al actualizar el registro de {typeof(TModel).Name}", ex);
            }
        }

        public async Task<bool> Delete(TModel model)
        {
            try
            {
                _dbcontext.Set<TModel>().Remove(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al eliminar el registro de {typeof(TModel).Name}", ex); ;
            }
        }
    }
}
