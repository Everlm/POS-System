
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Contexts;
using POS.Infrastructure.Persistences.Interfaces;

namespace POS.Infrastructure.Persistences.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly POSContext _context;

        public RoleRepository(POSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetRolesByIdsAsync(List<int> roleIds)
        {
            return await _context.Roles
                .Where(role => roleIds.Contains(role.RoleId))
                .ToListAsync();
        }
    }
}