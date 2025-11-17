using SocialMedia.Core.DTO.Role;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.DTO.Role;
using SocialMedia.Core.Entities.RoleEntity;

namespace SocialMedia.Core.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<Role?>?> GetAllRoleAsync();
        Task<Role?> GetRoleByIdAsync(string Id);
        Task<RetriveRoleDTO?> AddRoleAsync(RoleDTO dto);
        Task<RetriveRoleDTO?> UpdateRoleAsync(string id, RoleDTO dto);
        Task<bool> DeleteRoleAsync(string Id);
        Task<string> GetRoleIdUserAsync();
    }
}
