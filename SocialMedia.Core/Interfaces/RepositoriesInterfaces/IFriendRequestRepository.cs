using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IFriendRequestRepository
    {
        Task<IEnumerable<FriendRequest>> GetAllFriendRequests();
        Task<FriendRequest> GetFriendRequestById(int id);
        Task AddFriendRequest(FriendRequest FriendRequest);
        Task UpdateFriendRequest(FriendRequest FriendRequest);
        Task DeleteFriendRequest(int id);
        Task<IEnumerable<FriendRequest>> GetFriendRequestByReceiverID(string id);
        Task<IEnumerable<FriendRequest>> GetFriendRequestByUserID(string id);
    }
}
