using POS.Application.Commons.Bases.Request;
using System.Linq.Expressions;

namespace POS.Application.Commons.Filters
{
    public interface IFilterService
    {
        IQueryable<T> ApplyFilters<T>(
        IQueryable<T> query,
        BaseFiltersRequest filters,
        Dictionary<int, Expression<Func<T, bool>>>? customFilters = null);
    }
}
