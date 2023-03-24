using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(POSContext context) : base(context) { }

        public async Task<BaseEntityResponse<Client>> ListClients(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<Client>();

            var clients = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                  .Include(x => x.DocumentType)
                  .AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        clients = clients.Where(x => x.Name.Contains(filters.TextFilter));
                        break;

                    case 2:
                        clients = clients.Where(x => x.DocumentType.Name.Contains(filters.TextFilter));
                        break;

                    case 3:
                        clients = clients.Where(x => x.DocumentNumber.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                clients = clients.Where(x => x.State.Equals(filters.StateFilter));
            }

            if (filters.StartDate is not null && filters.EndDate is not null)
            {
                clients = clients.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                                x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await clients.CountAsync();
            response.Items = await Ordering(filters, clients, !(bool)filters.Download!).ToListAsync();
            return response;

        }
    }

}

