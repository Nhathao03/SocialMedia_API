using SocialMedia.Core.Entities.RoleEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IRoleCheckRepository
    {
        Task<IEnumerable<RoleCheck>> GetAllRoleCheck();
        Task<RoleCheck?> GetRoleCheckById(int Id);
        Task AddRoleCheck(RoleCheck roleCheck);
        Task UpdateRoleCheck(RoleCheck roleCheck);
        Task DeleteRoleCheck(int Id);
        Task DeleteRoleCheckByUserId(string userId);
        Task<bool> IsAdminAsync(string userId);
    }
}
