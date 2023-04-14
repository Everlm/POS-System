using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IBranchOfficeRepository :IGenericRepository<BranchOffice>
    {
        Task<BaseEntityResponse<BranchOffice>> ListBranchOffices(BaseFiltersRequest filters);
    }
}
