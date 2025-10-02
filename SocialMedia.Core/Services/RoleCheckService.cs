using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities.DTO.RoleCheck;
using SocialMedia.Core.Entities.RoleEntity;

namespace SocialMedia.Core.Services
{
    public class RoleCheckService : IRoleCheckService
    {
        private readonly IRoleCheckRepository _roleCheckRepository;

        public RoleCheckService(IRoleCheckRepository repository)
        {
            _roleCheckRepository = repository;
        }

        public async Task<IEnumerable<RoleCheck>> GetAllRoleCheckAsync()
        {
            return await _roleCheckRepository.GetAllRoleCheck();
        }

        public async Task<RoleCheck> GetRoleCheckByIdAsync(int id)
        {
            return await _roleCheckRepository.GetRoleCheckById(id);
        }

        public async Task AddRoleCheckAsync(RoleCheckDTO modelDTO)
        {
            var roleCheckEntity = new RoleCheck
            {
                Id = modelDTO.Id,
                UserID = modelDTO.UserId,
                RoleID = modelDTO.RoleId
            };
            await _roleCheckRepository.AddRoleCheck(roleCheckEntity);
        }

        public async Task UpdateRoleCheckAsync(RoleCheckDTO modelDTO)
        {
            var existingroleCheck = await _roleCheckRepository.GetRoleCheckById(modelDTO.Id);
            if (existingroleCheck == null)
            {
                throw new KeyNotFoundException($"RoleCheck with Id {modelDTO.Id} not exits.");
            }
            existingroleCheck.RoleID = modelDTO.RoleId;

            await _roleCheckRepository.UpdateRoleCheck(existingroleCheck);
        }

        public async Task DeleteRoleCheckAsync(int id)
        {
            await _roleCheckRepository.DeleteRoleCheck(id);
        }
        public async Task DeleteRoleCheckByUserIdAsync(string userId)
        {
            await _roleCheckRepository.DeleteRoleCheckByUserId(userId);
        }
        public async Task<bool> IsAdminAsync(string userId)
        {
            return await _roleCheckRepository.IsAdminAsync(userId);
        }
    }
}
