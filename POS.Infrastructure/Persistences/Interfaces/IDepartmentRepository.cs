using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<BaseEntityResponse<Department>> ListDepartments(BaseFiltersRequest filters);
    }
}
