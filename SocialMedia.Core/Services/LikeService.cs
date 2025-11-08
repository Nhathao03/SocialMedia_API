using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Core.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LikeService> _logger;
        private readonly IMapper _mapper;

        public LikeService(IUnitOfWork unitOfWork,
            ILogger<LikeService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task AddReactionAsync(LikeDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || dto.EntityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                throw new ArgumentException("Invalid input data");
            }
            var like = _mapper.Map<Like>(dto);
            await _unitOfWork.LikePostRepository.AddReactionAsync(like);
        }

        public async Task<bool> RemoveReactionAsync(LikeDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || dto.EntityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                throw new ArgumentException("Invalid input data");
            }
            var like = _mapper.Map<Like>(dto);
            return await _unitOfWork.LikePostRepository.RemoveReactionAsync(like);
        }

        public async Task<bool> ToggleReactionAsync(LikeDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || dto.EntityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                throw new ArgumentException("Invalid input data");
            }
            var like = _mapper.Map<Like>(dto);
            return await _unitOfWork.LikePostRepository.ToggleReactionAsync(like);
        }

        public async Task<int> GetReactionCountAsync(int entityId, EntityTypeEnum entity)
        {
            if (entityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                throw new ArgumentException("Invalid input data");
            }
            return await _unitOfWork.LikePostRepository.GetReactionCountAsync(entityId, entity);
        }

        public async Task<bool> HasUserReactionAsync(LikeDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || dto.EntityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                throw new ArgumentException("Invalid input data");
            }
            var like = _mapper.Map<Like>(dto);
            return await _unitOfWork.LikePostRepository.HasUserReactionAsync(like);
        }

        public async Task<List<string?>> GetUsersReactionAsync(int entityId, EntityTypeEnum entity)
        {
            if (entityId <= 0)
            {
                _logger.LogWarning("Invalid input data");
                throw new ArgumentException("Invalid input data");
            }
            return await _unitOfWork.LikePostRepository.GetUsersReactionAsync(entityId, entity);
        }
    }
}
