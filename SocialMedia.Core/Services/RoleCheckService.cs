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

        public async Task<RoleCheck> GetRoleCheckByIdAsync(int Id)
        {
            return await _roleCheckRepository.GetRoleCheckById(Id);
        }

        public async Task AddRoleCheckAsync(RoleCheckDTO modelDTO)
        {
            var roleCheckEntity = new RoleCheck
            {
                Id = modelDTO.Id,
                UserId = modelDTO.UserId,
                RoleId = modelDTO.RoleId
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
            existingroleCheck.RoleId = modelDTO.RoleId;

            await _roleCheckRepository.UpdateRoleCheck(existingroleCheck);
        }

        public async Task DeleteRoleCheckAsync(int Id)
        {
            await _roleCheckRepository.DeleteRoleCheck(Id);
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
