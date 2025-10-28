using SocialMedia.Infrastructure.Repositories;
using Social_Media.Helpers;
using System.Reflection.Metadata;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace SocialMedia.Core.Services
{
    public class FriendService : IFriendsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FriendService> _logger;

        public FriendService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FriendService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RetriveFriendDTO?> UpdateFriendsAsync(int id, FriendDTO dto)
        {
            _logger.LogInformation("Updating friend with ID {FriendId}", id);
            var existingFriend = await _unitOfWork.FriendRepository.GetFriendByIdAsync(id);
            if(existingFriend is null)
            {
                throw new KeyNotFoundException($"Friend with Id {id} not exists.");
            }

            var friend = _mapper.Map(dto, existingFriend);
            var result = await _unitOfWork.FriendRepository.UpdateFriendAsync(friend);
            _logger.LogInformation("Friend updated with ID {FriendId}", result?.ID);
            return _mapper.Map<RetriveFriendDTO>(result);
        }

        public async Task<bool> DeleteFriendsAsync(int id)
        {
            _logger.LogInformation("Deleting friend with ID {FriendId}", id);
            var existingFriend = await _unitOfWork.FriendRepository.GetFriendByIdAsync(id);
            if (existingFriend is null)
            {
                throw new KeyNotFoundException($"Friend with Id {id} not exists.");
            }

            var result = await _unitOfWork.FriendRepository.DeleteFriendAsync(id);
            _logger.LogInformation("Friend deleted with ID {FriendId}", id);
            return result;
        }

        public async Task<List<Friends>?> GetFriendRecentlyAddedAsync(string userID)
        {
           return await _unitOfWork.FriendRepository.GetFriendRecentlyAddedAsync(userID);
        }

        public async Task<List<Friends>?> GetFriendOfEachUserAsync(string userId)
        {
            return await _unitOfWork.FriendRepository.GetFriendOfEachUserAsync(userId);
        }

        public async Task<List<Friends>?> GetFriendBaseOnHomeTownAsync(string userId)
        {
            return await _unitOfWork.FriendRepository.GetFriendBaseOnHomeTownAsync(userId);
        }
    }
}
