using Common.Libraries.Services.Entities;
using Common.Libraries.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.EFCore.UnitOfWork
{
    public class UnitOfWorkRepository<T, Context> : IRepository<T> where T : class, IEntity where Context : DbContext
    {
        private readonly Context _dbContext;

        public UnitOfWorkRepository(Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string[] includeString = null,
            bool disableTracking = true)
        {
            var query = _dbContext.Set<T>().Where(predicate);
            if (includeString != null)
            {

                foreach (var inc in includeString)
                {
                    query = query.Include(inc);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!disableTracking)
            {
                query = query.AsNoTracking();
            }
            return await query?.FirstOrDefaultAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);

            return entity;
        }

        public async Task<int> DeleteAsync(T entity)
        {

            _dbContext.Set<T>().Remove(entity);
            return 1;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }


        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true)
        {

            var query = _dbContext.Set<T>().Where(predicate);
            includes.ForEach(include =>
            {
                query.Include(include);

            });
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!disableTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }



        public async Task<IReadOnlyList<T>> GetPaginatedByCondtionAsync(Expression<Func<T, bool>> predicate, int page, int size, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true)
        {

            page = page != 0 ? page - 1 : page;
            var query = _dbContext.Set<T>().Where(predicate).Take(size).Skip(page * size);
            if (includeString != null)
            {

                foreach (var inc in includeString)
                {
                    query = query.Include(inc);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!disableTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }


        public async Task<int> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return 1;
        }



        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string[] includeString = null, bool disableTracking = true)
        {
            var query = _dbContext.Set<T>().Where(predicate);
            if (includeString != null)
            {

                foreach (var inc in includeString)
                {
                    query = query.Include(inc);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!disableTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetPaginatedAsync(int page, int size, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true)
        {
            page = page != 0 ? page - 1 : page;
            IQueryable<T> query = _dbContext.Set<T>().Take(5).Skip(page * size);
            if (includeString != null)
            {

                foreach (var inc in includeString)
                {
                    query = query.Include(inc);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!disableTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return await _dbContext.Set<T>().CountAsync(predicate);
            else
                return await _dbContext.Set<T>().CountAsync();
        }
    }
}
