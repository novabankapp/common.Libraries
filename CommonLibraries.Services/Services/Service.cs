
using Common.Libraries.Services.Dtos;
using Common.Libraries.Services.Entities;
using Common.Libraries.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.Services
{
    public class Service<T, TD> : IService<T, TD> where T : class, IEntity where TD : IDTO
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> CreateAsync(T entity)
        {

            return await _repository.AddAsync(entity);

        }

        public async Task<ICollection<T>> CreateManyAsync(ICollection<T> entities)
        {
            var list = new List<T>();
            foreach (var ent in entities)
            {

                var entity = await _repository.AddAsync(ent);

                list.Add(entity);
            }
            return list;
        }

        public async Task<int> DeleteAsync(T entity)
        {

            return await _repository.DeleteAsync(entity);
        }

        public async Task<int> DeleteManyAsync(Expression<Func<T, bool>> predicate)
        {

            var entities = await _repository.GetAsync(predicate);
            var result = 0;
            foreach (var ent in entities)
            {
                var res = await _repository.DeleteAsync(ent);
                result += res;
            }
            return result;
        }
        public async Task<ICollection<T>> GetAsync()
        {
            var result = await _repository.GetAllAsync();
            return result.ToList();

        }
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string[] includeString = null,
            bool disableTracking = true)
        {
            return await _repository.GetOneAsync(predicate, orderBy, includeString, disableTracking);
        }

        public async Task<ICollection<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {

            var result = await _repository.GetAsync(predicate);
            return result.ToList();

        }

        public async Task<ICollection<T>> GetPaginatedByConditionAsync(Expression<Func<T, bool>> predicate, int page, int size,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true)
        {

            var res = await _repository.GetPaginatedByCondtionAsync(predicate, page, size, orderBy, includeString, disableTracking);
            return res.ToList();
        }

        public async Task<ICollection<T>> GetPaginatedAsync(int page, int size,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string[] includeString = null, bool disableTracking = true)
        {

            var res = await _repository.GetPaginatedAsync(page, size, orderBy, includeString, disableTracking);
            return res.ToList();
        }

        public async Task<int> UpdateAsync(T entity)
        {

            return await _repository.UpdateAsync(entity);
        }

        public async Task<int> UpdateManyAsync(ICollection<T> entities)
        {
            var res = 0;
            foreach (var ent in entities)
            {

                var result = await _repository.UpdateAsync(ent);
                res = result + res;
            }
            return res;
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return _repository.CountAsync(predicate);
        }
    }
}
