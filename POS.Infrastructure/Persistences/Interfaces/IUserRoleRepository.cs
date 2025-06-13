
using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IUserRoleRepository
    {
        void RemoveRange(IEnumerable<UserRole> userRoles);
        Task AddRangeAsync(IEnumerable<UserRole> userRoles);
        Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(int userId);
    }
}