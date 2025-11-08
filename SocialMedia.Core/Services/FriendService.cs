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

        public async Task<FriendShipStatus> CheckFriendshipAsync(string userId, string targetUserId)
        {
            if(userId == null || targetUserId == null)
            {
                _logger.LogWarning("Invalid input data");
                throw new ArgumentNullException(nameof(userId), "userId and targetUserId can not empty");
            }
                
            var request = await _unitOfWork.FriendRequestRepository.GetFriendRequestBetweenUsersAsync(userId, targetUserId);
            if(request == null) 
                return FriendShipStatus.None;
            return request.status switch
            {
                (int)Constants.FriendRequestStatus.Pending => FriendShipStatus.Pending,
                (int)Constants.FriendRequestStatus.Accepted => FriendShipStatus.Friends,
                (int)Constants.FriendRequestStatus.Rejected => FriendShipStatus.Rejected,
                _ => FriendShipStatus.None,
            };
        }

        public async Task<bool> DeleteFriendsAsync(string userId, string userB)
        {
            _logger.LogInformation("Deleting friend with userId {userId} and targetUserId {targetUserId}", userId, userB);
            var existingFriendrequest = await _unitOfWork.FriendRequestRepository.GetFriendRequestBetweenUsersAsync(userId, userB);
            if (existingFriendrequest is null)
            {
                throw new KeyNotFoundException($"Friend request with user {userId} and userB {userB} not exists.");
            }

            existingFriendrequest.status = (int)Constants.FriendRequestStatus.Accepted;
            await _unitOfWork.FriendRequestRepository.UpdateFriendRequestAsync(existingFriendrequest);

            var existingFriend = await _unitOfWork.FriendRepository.GetFriendAsync(userId, userB);
            if (existingFriend is null) 
            {
                throw new KeyNotFoundException($"Friend with user {userId} and userB {userB} not exists.");
            }

            var result = await _unitOfWork.FriendRepository.DeleteFriendAsync(existingFriend.ID);
            return result;
        }
    }
}
