using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class BusinessRepository : GenericRepository<Business>, IBusinessRepository
    {
        public BusinessRepository(POSContext context) : base(context) { }

        public async Task<BaseEntityResponse<Business>> ListBusiness(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<Business>();

            var business = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                .Include(d => d.District)
                .AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        business = business.Where(x => x.BusinessName!.Contains(filters.TextFilter));
                        break;

                    case 2:
                        business = business.Where(x => x.Ruc!.Contains(filters.TextFilter));
                        break;
                    case 3:
                        business = business.Where(x => x.District.Name!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                business = business.Where(x => x.State.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                business = business.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                                x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await business.CountAsync();
            response.Items = await Ordering(filters, business, !(bool)filters.Download!).ToListAsync();
            return response;
        }
    }
}
