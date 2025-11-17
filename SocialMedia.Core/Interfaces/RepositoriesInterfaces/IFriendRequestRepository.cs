using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IFriendRequestRepository
    {
        Task<List<FriendRequest>?> GetAllFriendRequestAsync();
        Task<FriendRequest?> GetFriendRequestByIdAsync(int Id);
        Task<FriendRequest?> AddFriendRequestAsync(FriendRequest FriendRequest);
        Task<FriendRequest?> UpdateFriendRequestAsync(FriendRequest FriendRequest);
        Task<bool> DeleteFriendRequestAsync(int Id);
        Task<List<FriendRequest>?> GetFriendRequestByUserIdAsync(string Id);
        Task<List<FriendRequest>?> GetReceivedRequestAsync(string userId);
        Task<List<FriendRequest>?> GetSentRequestAsync(string userId);
        Task<FriendRequest?> GetFriendRequestBetweenUsersAsync(string userA, string userB);
    }
}
