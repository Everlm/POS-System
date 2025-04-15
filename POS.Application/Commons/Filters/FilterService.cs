using System.Linq.Expressions;
using POS.Application.Commons.Bases.Request;

namespace POS.Application.Commons.Filters;

public class FilterService : IFilterService
{
    public IQueryable<T> ApplyFilters<T>(IQueryable<T> query, BaseFiltersRequest filters, Dictionary<int, Expression<Func<T, bool>>>? customFilters = null)
    {
        if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter) && customFilters != null)
        {
            if (customFilters.TryGetValue(filters.NumFilter.Value, out var filterExpression))
            {
                query = query.Where(filterExpression);
            }
        }

        if (filters.StateFilter is not null && typeof(T).GetProperty("State") != null)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(param, "State");
            var constant = Expression.Constant(filters.StateFilter);
            var condition = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(condition, param);
            query = query.Where(lambda);
        }

        if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate) &&
            DateTime.TryParse(filters.StartDate, out var startDate) &&
            DateTime.TryParse(filters.EndDate, out var endDate) &&
            typeof(T).GetProperty("AuditCreateDate") != null)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(param, "AuditCreateDate");
            var lower = Expression.GreaterThanOrEqual(property, Expression.Constant(startDate));
            var upper = Expression.LessThanOrEqual(property, Expression.Constant(endDate.AddDays(1)));
            var combined = Expression.AndAlso(lower, upper);
            var lambda = Expression.Lambda<Func<T, bool>>(combined, param);
            query = query.Where(lambda);
        }

        return query;
    }

}
