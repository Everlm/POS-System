
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly POSContext _context;
        public UserRoleRepository(POSContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<UserRole> userRoles)
        {
            await _context.UserRoles.AddRangeAsync(userRoles);
        }

        public void RemoveRange(IEnumerable<UserRole> userRoles)
        {
            _context.UserRoles.RemoveRange(userRoles);
        }

        public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();
        }
    }
}