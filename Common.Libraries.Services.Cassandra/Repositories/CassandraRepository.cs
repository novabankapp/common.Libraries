using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Common.Libraries.Services.Entities;
using Common.Libraries.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.Cassandra.Repositories
{
    public class CassandraRepository<T, Session> : IRepository<T> where T : class, IEntity where Session : ISession
    {
        private ISession _session;
        private IMapper _mapper;
        private Table<T> _table;
        public CassandraRepository(ISession session)
        {
            _session = session;
            _mapper = new Mapper(_session);
            _table = new Table<T>(_session);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _mapper.InsertAsync(entity);
            return entity;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            await _mapper.DeleteAsync(entity);
            return 1;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var result = await _mapper.FetchAsync<T>();
            return result.ToList();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _table.Where(predicate).ExecuteAsync();
            return result.ToList();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true)
        {
             
            var query = _table.Where(predicate);
           
            if(orderBy != null)
            {
                query = (CqlQuery<T>)orderBy(query);
            }
            var result = await query.ExecuteAsync();
            return result.ToList();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            var query = _table.Where(predicate);

            if (orderBy != null)
            {
                query = (CqlQuery<T>)orderBy(query);
            }
            var result = await query.ExecuteAsync();
            return result.ToList();
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true)
        {
            var query = _table.Where(predicate);

            if (orderBy != null)
            {
                query = (CqlQuery<T>)orderBy(query);
            }
            var result = await query.ExecuteAsync();
            return result.FirstOrDefault();
        }

        public async Task<IReadOnlyList<T>> GetPaginatedAsync(int page, int size, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true)
        {
            page = page != 0 ? page - 1 : page;
            var query = _table.Where(i => true);

            if (orderBy != null)
            {
                query = (CqlQuery<T>)orderBy(query);
            }
            var result = await query.ExecuteAsync();
            return result.Skip(page).Take(size).ToList();
        }

        public async Task<IReadOnlyList<T>> GetPaginatedByCondtionAsync(Expression<Func<T, bool>> predicate, int page, int size, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true)
        {
            page = page != 0 ? page - 1 : page;
            var query = _table.Where(predicate);

            if (orderBy != null)
            {
                query = (CqlQuery<T>)orderBy(query);
            }
            var result = await query.ExecuteAsync();
            return result.Skip(page).Take(size).ToList();
        }

        public async Task<int> UpdateAsync(T entity)
        {
           await _mapper.UpdateAsync(entity);
            return 1;
        }
    }
}
