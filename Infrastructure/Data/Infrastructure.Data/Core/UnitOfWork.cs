using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Data.Core
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private bool disposed = false;
        public UnitOfWork(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new EfRepository<TEntity>(_dbContext);
        }

        public TRepository GetRepository<TEntity, TRepository>()
            where TEntity : class
            where TRepository : EfRepository<TEntity>
        {
            //var repository = Extensions.UnitOfWorkExtension.Repositorys[typeof(IRepository<TEntity>)] as TRepository;
            //return Activator.CreateInstance(Extensions.UnitOfWorkExtension.Repositorys[typeof(TEntity)], _dbContext) as TRepository;
            return Activator.CreateInstance(typeof(TRepository), _dbContext) as TRepository;
        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose the db context.
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
