
using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRolesByIdsAsync(List<int> roleIds);
    }
}