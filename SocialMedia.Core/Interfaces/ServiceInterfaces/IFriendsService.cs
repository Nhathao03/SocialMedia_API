using Microsoft.VisualBasic;
using Social_Media.Helpers;
using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Core.Services
{
    public interface IFriendsService
    {
        Task<FriendShipStatus> CheckFriendshipAsync(string userId, string targetUserId);
        Task<List<Friends>?> GetFriendRecentlyAddedAsync(string  userId);
        Task<List<Friends>?> GetFriendOfEachUserAsync(string userId);
        Task<List<Friends>?> GetFriendBaseOnHomeTownAsync(string userId);
        Task<bool> DeleteFriendsAsync(string userA, string userB);

    }
}