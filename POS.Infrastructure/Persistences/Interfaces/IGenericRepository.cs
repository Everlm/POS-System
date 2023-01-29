using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using System.Linq.Expressions;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> RegisterAsync(T entity);
        Task<bool> EditAsync(T entity);
        Task<bool> DeleteAsync(int id);
        IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null);
        public IQueryable<TDTO> Ordering<TDTO>(BasePaginationRequest request, IQueryable<TDTO> queryable, bool pagination = false) where TDTO : class;
    }
}
