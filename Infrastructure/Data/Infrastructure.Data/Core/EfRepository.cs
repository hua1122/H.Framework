using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Core
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public EfRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }
        public virtual IQueryable<T> Entities
        {
            get
            {
                return _dbSet;
            }
        }
        public virtual IQueryable<T> EntitiesNoTracking => _dbSet.AsNoTracking();

        #region Sync
        public virtual bool Add(T entity)
        {
            //return _dbSet.Add(entity).State == EntityState.Added;
            _dbSet.Add(entity);
            return true;
        }
        public virtual bool AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return true;
        }
        public virtual bool Delete(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }
        public virtual bool DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return true;
        }
        public virtual bool Update(T entity)
        {
            _dbSet.Update(entity);
            return true;
        }
        public virtual bool UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return true;
        }
        public virtual T FindById(dynamic id) => _dbSet.Find(id);
        public virtual int Count(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet.Count();
            }
            else
            {
                return _dbSet.Count(predicate);
            }
        }
        public virtual T Get(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet.FirstOrDefault();
            }
            else
            {
                return _dbSet.FirstOrDefault(predicate);
            }
        }
        public virtual IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet.ToList();
            }
            else
            {
                return _dbSet.Where(predicate).ToList();
            }
        }
        public virtual Tuple<IEnumerable<T>, int> GetPage(Expression<Func<T, bool>> predicate = null, Expression<Func<T, dynamic>> orderBy = null, bool isDesc = false, int pageIndex = 1, int pageSize = 20)
        {
            //var query = Get(predicate);
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                if (isDesc)
                {
                    query = query.OrderByDescending(orderBy);
                }
                else
                {
                    query = query.OrderBy(orderBy);
                }
            }
            var total = query.Count();
            return new Tuple<IEnumerable<T>, int>(query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList(), total);
        }
        public virtual IEnumerable<T> SqlQuery(string sql, IEnumerable<SqlParameter> parms)
        {
            throw new NotImplementedException();
        }
        public virtual int ExecuteSqlCommand(string sql, IEnumerable<SqlParameter> parms)
        {
            throw new NotImplementedException();
        }

        #endregion Sync

        #region Async
        public virtual async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }
        public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return true;
        }
        public virtual async Task<T> FindByIdAsync(dynamic id) => await _dbSet.FindAsync(id);
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            else
            {
                return await _dbSet.CountAsync(predicate);
            }
        }
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.FirstOrDefaultAsync();
            }
            else
            {
                return await _dbSet.FirstOrDefaultAsync(predicate);
            }
        }
        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.ToListAsync();
            }
            else
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
        }
        public virtual async Task<Tuple<IEnumerable<T>, int>> GetPageAsync(Expression<Func<T, bool>> predicate = null, Expression<Func<T, dynamic>> orderBy = null, bool isDesc = false, int pageIndex = 1, int pageSize = 20)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                if (isDesc)
                {
                    query = query.OrderByDescending(orderBy);
                }
                else
                {
                    query = query.OrderBy(orderBy);
                }
            }
            var total = await query.CountAsync();
            return new Tuple<IEnumerable<T>, int>(await query.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync(), total);
        }       

        #endregion Async
    }
}
