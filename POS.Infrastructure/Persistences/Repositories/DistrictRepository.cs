using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class DistrictRepository : GenericRepository<District>, IDistrictRepository
    {
        public DistrictRepository(POSContext context) : base(context) { }


        public async Task<BaseEntityResponse<District>> ListDistrics(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<District>();

            var district = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                .Include(p => p.Province)
                .AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        district = district.Where(x => x.Name!.Contains(filters.TextFilter));
                        break;

                    case 2:
                        district = district.Where(x => x.Province.Name.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                district = district.Where(x => x.State.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                district = district.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                                x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await district.CountAsync();
            response.Items = await Ordering(filters, district, !(bool)filters.Download!).ToListAsync();
            return response;
        }
    }
}
