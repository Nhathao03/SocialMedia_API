using SocialMedia.Infrastructure.Repositories;
using Social_Media.Helpers;
using SocialMedia.Core.Entities.DTO;
using System.Reflection.Metadata;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Core.Services
{
    public class FriendService : IFriendsService
    {
        private readonly IFriendRepository _FriendRepository;
        private readonly IFriendRequestRepository _FriendRequestRepository;

        public FriendService(IFriendRepository repository,
            IFriendRequestRepository friendRequestRepository)
        {
            _FriendRepository = repository;
            _FriendRequestRepository = friendRequestRepository;
        }

        public async Task<IEnumerable<Friends>> GetAllFriendsAsync()
        {
            return await _FriendRepository.GetAllFriends();
        }

        public async Task<Friends> GetFriendsByIdAsync(int id)
        {
            return await _FriendRepository.GetFriendById(id);
        }

        public async Task AddFriendsAsync(int id)
        {
            var getFriendRequest = await _FriendRequestRepository.GetFriendRequestById(id);
            var data = new Friends
            {
                FriendID = getFriendRequest.ReceiverID,
                UserID = getFriendRequest.SenderID,
                Type_FriendsID = (int)Constants.FriendsEnum.normal,
            };

            await _FriendRepository.AddFriend(data);
        }

        public async Task UpdateFriendsAsync(FriendDTO modelDTO)
        {
            var Friend = await _FriendRepository.GetFriendById(modelDTO.Id);
            if (Friend == null)
            {
                throw new KeyNotFoundException($"Friend with ID {modelDTO.Id} not found.");
            }
            Friend.FriendID = modelDTO.FriendID;
            Friend.UserID = modelDTO.userID;
            await _FriendRepository.UpdateFriend(Friend);
        }

        public async Task DeleteFriendsAsync(int id)
        {
            await _FriendRepository.DeleteFriend(id);
        }

        public async Task<IEnumerable<Friends>> GetFriendsByUserIDAsync(string userID)
        {
            return await _FriendRepository.GetFriendsByUserID(userID);
        }

        public async Task<IEnumerable<Friends>> getFriendRecentlyAdded(string userID)
        {
           return await _FriendRepository.getFriendRecentlyAdded(userID);
        }

        public async Task<IEnumerable<string>> getFriendOfEachUser(string userId)
        {
            return await _FriendRepository.GetFriendOfEachUser(userId);
        }

        public async Task<IEnumerable<Friends>> GetFriendBaseOnHomeTown(string userId)
        {
            return await _FriendRepository.GetFriendBaseOnHomeTown(userId);
        }
    }
}
