using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.RoleEntity;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role?>?> GetAllRole()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleById(string Id)
        {
            return await _context.Roles.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<Role?> AddRole(Role role)
        {
            _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role?> UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRole(string Id)
        {
            var role = await _context.Roles.FindAsync(Id);
            if (role is null)
                return false;
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> GetRoleIdUser()
        {
            var user = await _context.Roles.FirstOrDefaultAsync(u => u.Name == "User");
            if (user == null) return null;

            return user.Id.ToString();
        }
    }
}
