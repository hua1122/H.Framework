using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    /// <summary>
    /// CRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        bool Add(T entity);
        Task<bool> AddAsync(T entity);
        bool AddRange(IEnumerable<T> entities);
        Task<bool> AddRangeAsync(IEnumerable<T> entities);
        bool Delete(T entity);
        bool DeleteRange(IEnumerable<T> entities);
        bool Update(T entity);
        bool UpdateRange(IEnumerable<T> entities);
        T FindById(dynamic id);
        Task<T> FindByIdAsync(dynamic id);
        int Count(Expression<Func<T, bool>> predicate = null);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        T Get(Expression<Func<T, bool>> predicate = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null);
        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null);
        Tuple<IEnumerable<T>, int> GetPage(Expression<Func<T, bool>> predicate = null, Expression<Func<T, dynamic>> orderBy = null, bool isDesc = false, int pageIndex = 1, int pageSize = 20);
        Task<Tuple<IEnumerable<T>, int>> GetPageAsync(Expression<Func<T, bool>> predicate = null, Expression<Func<T, dynamic>> orderBy = null, bool isDesc = false, int pageIndex = 1, int pageSize = 20);
        IEnumerable<T> SqlQuery(string sql, IEnumerable<SqlParameter> parms);
        int ExecuteSqlCommand(string sql, IEnumerable<SqlParameter> parms);
    }
}
