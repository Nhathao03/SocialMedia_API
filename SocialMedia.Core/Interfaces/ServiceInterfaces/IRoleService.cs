using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.DTO.Role;
using SocialMedia.Core.Entities.RoleEntity;

namespace SocialMedia.Core.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRoleAsync();
        Task<Role> GetRoleByIdAsync(string id);
        Task AddRoleAsync(string RoleName);
        Task UpdateRoleAsync(RoleDTO modelDTO);
        Task DeleteRoleAsync(int id);
        Task<string> GetRoleIdUserAsync();

    }
}
