using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Core.Services
{
    public interface IFriendsService
    {
        Task<RetriveFriendDTO?> UpdateFriendsAsync(int id, FriendDTO dto);
        Task<bool> DeleteFriendsAsync(int id);
        Task<List<Friends>?> GetFriendRecentlyAddedAsync(string  userId);
        Task<List<Friends>?> GetFriendOfEachUserAsync(string userId);
        Task<List<Friends>?> GetFriendBaseOnHomeTownAsync(string userId);
    }
}