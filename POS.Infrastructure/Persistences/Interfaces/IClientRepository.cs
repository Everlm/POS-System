using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client> 
    {
        Task<BaseEntityResponse<Client>> ListClients(BaseFiltersRequest filters);
    }
}
