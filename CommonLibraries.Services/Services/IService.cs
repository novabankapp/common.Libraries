
using Common.Libraries.Services.Dtos;
using Common.Libraries.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.Services
{
    public interface IService<T, TD> where T : IEntity where TD : IDTO 
    {
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> GetOneAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string[] includeString = null,
            bool disableTracking = true);
        Task<T> CreateAsync(T entity);
       

        Task<ICollection<T>> GetPaginatedAsync(int page, int size,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true);

        Task<ICollection<T>> CreateManyAsync(ICollection<T> entity);
        Task<int> DeleteAsync(T entity);

        Task<int> DeleteManyAsync(Expression<Func<T, bool>> predicate);

        Task<int> UpdateAsync(T dto);

        Task<int> UpdateManyAsync(ICollection<T> entities);
        Task<ICollection<T>> GetAsync();

        Task<ICollection<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> GetPaginatedByConditionAsync(
            Expression<Func<T, bool>> predicate,
            int page,
            int size,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string[] includeString = null,
            bool disableTracking = true
            );
    }
}