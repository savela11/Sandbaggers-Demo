using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);

        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);

        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null);

        Task<T> AddAsync(T entity);
        Task AddNoReturn(T entity);

        Task RemoveAsync(int id);
        Task RemoveAsync(T entity);


        Task RemoveNoReturn(int id);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);
    }
}
