using SocialMedia.Core.Entities.RoleEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IRoleCheckRepository
    {
        Task<IEnumerable<RoleCheck>> GetAllRoleCheck();
        Task<RoleCheck> GetRoleCheckById(int id);
        Task AddRoleCheck(RoleCheck roleCheck);
        Task UpdateRoleCheck(RoleCheck roleCheck);
        Task DeleteRoleCheck(int id);
        Task DeleteRoleCheckByUserId(string userId);
        Task<bool> IsAdminAsync(string userId);
    }
}
