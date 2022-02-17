using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Carrot.Contracts.Repositories;
using Carrot.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carrot.Repositories.Common
{
    public abstract class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
    {
        protected Context Context { get; set; }
        protected RepositoryBase(Context context) => Context = context;
        public IEnumerable<T> FindAll() => Context.Set<T>();
        public async Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression) => await Context.Set<T>().Where(expression).ToListAsync();
        public IQueryable<T> FindByConditionAsNoTracking(Expression<Func<T, bool>> expression) => Context.Set<T>().Where(expression).AsNoTracking();
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression) => await Context.Set<T>().AnyAsync(expression);
        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> expression) => await Context.Set<T>().FirstOrDefaultAsync(expression);
        public async Task<T> GetFirstAsNoTrackingAsync(Expression<Func<T, bool>> expression) => await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        public async Task CreateAsync(T entity) => await Context.Set<T>().AddAsync(entity);
        public void Update(T entity) => Context.Set<T>().Update(entity);
        public void Delete(T entity) => Context.Set<T>().Remove(entity);
        public void DeleteRange(List<T> entities) => Context.Set<T>().RemoveRange(entities);
        public async Task<int> SaveAsync() => await Context.SaveChangesAsync();
        public async Task Refresh(T entity) => await Context.Entry(entity).ReloadAsync();
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
