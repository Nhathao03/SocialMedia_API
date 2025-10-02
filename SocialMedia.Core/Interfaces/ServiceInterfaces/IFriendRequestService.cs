using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Core.Services
{
    public interface IFriendRequestService
    {
        Task<IEnumerable<FriendRequest>> GetAllFriendRequestAsync();
        Task<FriendRequest> GetFriendRequestByIdAsync(int id);
        Task UpdateFriendRequestAsync(FriendRequestDTO FriendRequest);
        Task DeleteFriendRequestAsync(int id);
        Task AddFriendRequestAsync(FriendRequestDTO FriendRequest);
        Task<IEnumerable<FriendRequest>> GetFriendRequestByReceiverID(string id);
        Task<IEnumerable<FriendRequest>> GetFriendRequestByUserID(string id);
        Task ConfirmRequest(int id);

    }
}
