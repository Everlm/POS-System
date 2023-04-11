using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IProvinceRepository : IGenericRepository<Province>
    {
        Task<BaseEntityResponse<Province>> ListProvinces(BaseFiltersRequest filters);
    }
}
