
using Common.Libraries.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetOneAsync(Expression<Func<T, bool>> predicate = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
         string[] includeString = null,
          bool disableTracking = true);

       


        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string[] includeString = null,
            bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true);

        Task<IReadOnlyList<T>> GetPaginatedAsync(int page,
            int size, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string[] includeString = null, bool disableTracking = true);

       

        Task<IReadOnlyList<T>> GetPaginatedByCondtionAsync(Expression<Func<T, bool>> predicate,
            int page, int size,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string[] includeString = null,
           bool disableTracking = true);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        Task<T> AddAsync(T entity);

        Task<int> UpdateAsync(T entity);

        Task<int> DeleteAsync(T entity);
    }
}
