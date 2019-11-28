using System;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {       
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
