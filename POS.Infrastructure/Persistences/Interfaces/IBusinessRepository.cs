using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IBusinessRepository:IGenericRepository<Business>
    {
        Task<BaseEntityResponse<Business>> ListBusiness(BaseFiltersRequest filters);
    }
}
