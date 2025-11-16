using AutoMapper;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.DTO.Role;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.DTO.Role;
using SocialMedia.Core.Entities.RoleEntity;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       
        public RoleService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
		}

        public async Task<IEnumerable<Role?>?> GetAllRoleAsync()
        {
            return await _unitOfWork.RoleRepository.GetAllRole();
        }

        public async Task<Role?> GetRoleByIdAsync(string Id)
        {
            return await _unitOfWork.RoleRepository.GetRoleById(Id);
        }

        public async Task<RetriveRoleDTO?> AddRoleAsync(RoleDTO dto)
        {
			if (dto is null)
				throw new ArgumentNullException(nameof(RoleDTO), "Role data is required.");
			if (string.IsNullOrWhiteSpace(dto.Name))
				throw new ArgumentException("Role name cannot be empty.", nameof(dto.Name));
			var role = _mapper.Map<Role>(dto);
            role.Id = GenerateRoleId();
			var result = await _unitOfWork.RoleRepository.AddRole(role);
			var mapper = _mapper.Map<RetriveRoleDTO>(result);
            return mapper;
		}

        // Generate roleId 
        private string GenerateRoleId()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                             .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<RetriveRoleDTO?> UpdateRoleAsync(string Id, RoleDTO dto)
        {
            var existingRole = await _unitOfWork.RoleRepository.GetRoleById(Id);
            if(existingRole == null)
            {
                throw new KeyNotFoundException($"Role with Id {Id} not exits.");
            }

            var role = _mapper.Map(dto, existingRole);
			await _unitOfWork.RoleRepository.UpdateRole(existingRole);
            return _mapper.Map<RetriveRoleDTO>(role);
		}

        public async Task<bool> DeleteRoleAsync(string Id)
        {
            var exitsAddress = await _unitOfWork.RoleRepository.GetRoleById(Id);
			if (exitsAddress is null)
			{
				throw new KeyNotFoundException($"Role with Id {Id} not exits.");
			}

			var result = await _unitOfWork.RoleRepository.DeleteRole(Id);
			return result;
		}
        public async Task<string?> GetRoleIdUserAsync()
        {
            return await _unitOfWork.RoleRepository.GetRoleIdUser();
        }
    }
}
