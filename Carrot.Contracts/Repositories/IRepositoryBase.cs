using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Carrot.Contracts.Repositories
{
    public interface IRepositoryBase<T>
    {
        IEnumerable<T> FindAll();
        Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByConditionAsNoTracking(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> expression);
        Task<T> GetFirstAsNoTrackingAsync(Expression<Func<T, bool>> expression);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> entities);
        Task<int> SaveAsync();
        Task Refresh(T entity);
    }
}
