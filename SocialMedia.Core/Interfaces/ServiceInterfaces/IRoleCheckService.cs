using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.DTO.RoleCheck;
using SocialMedia.Core.Entities.RoleEntity;

namespace SocialMedia.Core.Services
{
    public interface IRoleCheckService
    {
        Task<IEnumerable<RoleCheck>> GetAllRoleCheckAsync();
        Task<RoleCheck> GetRoleCheckByIdAsync(int id);
        Task AddRoleCheckAsync(RoleCheckDTO roleCheck);
        Task UpdateRoleCheckAsync(RoleCheckDTO roleCheck);
        Task DeleteRoleCheckAsync(int id);
        Task DeleteRoleCheckByUserIdAsync(string userId);
        Task<bool> IsAdminAsync(string userId);    }
}
