using Infrastructure.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public interface IUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        TRepository GetRepository<TEntity, TRepository>()
            where TEntity : class
            where TRepository : EfRepository<TEntity>;
    }
}
