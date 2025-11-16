using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.Entities.FriendEntity;
using System.ComponentModel;

namespace SocialMedia.Core.Services
{
    public interface IFriendRequestService
    {
        Task<FriendRequest?> GetFriendRequestByIdAsync(int Id);
        Task<RetriveFriendRequestDTO?> SendFriendRequestAsync(FriendRequestDTO modelDto);
        Task<List<FriendRequest>?> GetFriendRequestByUserIdAsync(string Id);
        Task<List<FriendRequest>?> GetSentRequestAsync(string userId);
        Task<List<FriendRequest>?> GetReceivedRequestAsync(string userId);
        Task<RetriveFriendDTO?> AcceptFriendRequestAsync(int Id);
        Task RejectFriendRequestAsync(int Id);
        Task CancelFriendRequestAsync(int Id);
        Task<RetriveFriendRequestDTO?> UpdateFriendRequestAsync(int Id, FriendRequestDTO modelDto);
        Task<bool> DeleteFriendRequestAsync(int Id);
    }
}
