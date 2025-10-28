using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.Entities.FriendEntity;
using System.ComponentModel;

namespace SocialMedia.Core.Services
{
    public interface IFriendRequestService
    {
        Task<FriendRequest?> GetFriendRequestByIdAsync(int id);
        Task<RetriveFriendRequestDTO?> SendFriendRequestAsync(FriendRequestDTO modelDto);
        Task<List<FriendRequest>?> GetFriendRequestByUserIdAsync(string id);
        Task<List<FriendRequest>?> GetSentRequestAsync(string userId);
        Task<List<FriendRequest>?> GetReceivedRequestAsync(string userId);
        Task<RetriveFriendDTO?> AcceptFriendRequestAsync(int id);
        Task RejectFriendRequestAsync(int id);
        Task CancelFriendRequestAsync(int id);
        Task<RetriveFriendRequestDTO?> UpdateFriendRequestAsync(int id, FriendRequestDTO modelDto);
        Task<bool> DeleteFriendRequestAsync(int id);
    }
}
