using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.RoleEntity;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class RoleCheckRepository : IRoleCheckRepository
    {
        private readonly AppDbContext _context;
        private readonly IRoleRepository _roleRepository;

        public RoleCheckRepository(AppDbContext context,
            IRoleRepository roleRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleCheck>> GetAllRoleCheck()
        {
            return await _context.RoleChecks.ToListAsync();
        }

        public async Task<RoleCheck?> GetRoleCheckById(int Id)
        {
            return await _context.RoleChecks.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task AddRoleCheck(RoleCheck roleCheck)
        {
            _context.RoleChecks.AddAsync(roleCheck);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleCheck(RoleCheck roleCheck)
        {
            _context.RoleChecks.Update(roleCheck);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleCheck(int Id)
        {
            var roleCheck = await _context.RoleChecks.FindAsync(Id);
            if (roleCheck != null)
            {
                _context.RoleChecks.Remove(roleCheck);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRoleCheckByUserId(string userId)
        {
            var user = _context.RoleChecks.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                _context.RoleChecks.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsAdminAsync(string userId)
        {
            var adminRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name.Equals("Admin"));

            if (adminRole == null) return false; // No "Admin" role found

            // Check if user has any role assigned
            var userRoleCheck = await _context.RoleChecks
                .FirstOrDefaultAsync(rc => rc.UserId == userId);

            if (userRoleCheck == null || string.IsNullOrEmpty(userRoleCheck.RoleId))
                return false; // User has no role assigned, or role is NULL

            return userRoleCheck.RoleId == adminRole.Id.ToString();
        }
    }
}
