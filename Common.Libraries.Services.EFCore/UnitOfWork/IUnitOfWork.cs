using Common.Libraries.Services.Entities;
using Common.Libraries.Services.Repositories;
using Common.Libraries.Services.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.EFCore.UnitOfWork
{
    
    public class UnitOfWork<Context> : IUnitOfWork, IDisposable where Context : DbContext
    {
        private readonly Context _dbContext;
        public UnitOfWork(Context dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<int> Commit()
        {
            return _dbContext.SaveChangesAsync();
        }
        public void Rollback()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }
        public IRepository<T> Repository<T>() where T : class, IEntity
        {
            return new UnitOfWorkRepository<T,Context>(_dbContext);
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
