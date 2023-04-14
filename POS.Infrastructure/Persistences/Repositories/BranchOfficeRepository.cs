using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class BranchOfficeRepository : GenericRepository<BranchOffice>, IBranchOfficeRepository
    {
        public BranchOfficeRepository(POSContext context) : base(context) { }


        public async Task<BaseEntityResponse<BranchOffice>> ListBranchOffices(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<BranchOffice>();

            var branchOffice = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                .Include(b => b.Business)
                .Include(d => d.District)
                .Include(u => u.UsersBranchOffices)
                .AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        branchOffice = branchOffice.Where(x => x.Name.Contains(filters.TextFilter));
                        break;

                    case 2:
                        branchOffice = branchOffice.Where(x => x.Code.Contains(filters.TextFilter));
                        break;

                    case 3:
                        branchOffice = branchOffice.Where(x => x.District.Name.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                branchOffice = branchOffice.Where(x => x.State.Equals(filters.StateFilter));
            }

            if (filters.StartDate is not null && filters.EndDate is not null)
            {
                branchOffice = branchOffice.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                                x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await branchOffice.CountAsync();
            response.Items = await Ordering(filters, branchOffice, !(bool)filters.Download!).ToListAsync();
            return response;
        }
    }
}
