using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class ProvinceRepository : GenericRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(POSContext context) : base(context) { }


        public async Task<BaseEntityResponse<Province>> ListProvinces(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<Province>();

            var province = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null);

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        province = province.Where(x => x.Name!.Contains(filters.TextFilter));
                        break;

                    case 2:
                        province = province.Where(x => x.Department.Name.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                province = province.Where(x => x.State.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                province = province.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                                x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await province.CountAsync();
            response.Items = await Ordering(filters, province, !(bool)filters.Download!).ToListAsync();
            return response;
        }
    }
}
