 using SocialMedia.Core.Entities.RoleEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role?>?> GetAllRole();
        Task<Role?> GetRoleById(string Id);
        Task<Role?> AddRole(Role role);
        Task<Role?> UpdateRole(Role role);
        Task<bool> DeleteRole(string Id);
        Task<string?> GetRoleIdUser();
    }
}
