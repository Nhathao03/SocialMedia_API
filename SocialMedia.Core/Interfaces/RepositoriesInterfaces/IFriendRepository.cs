using Social_Media.Helpers;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IFriendRepository
    {
        Task<Friends?> GetFriendByIdAsync(int id);
        Task<Friends?> AddFriendAsync(Friends friends);
        Task<Friends?> UpdateFriendAsync(Friends friends);
        Task<bool> DeleteFriendAsync(int id);
        Task<List<Friends>?> GetFriendRecentlyAddedAsync(string userId);
        Task<List<Friends>?> GetFriendOfEachUserAsync(string userId);
        Task<List<Friends>?> GetFriendBaseOnHomeTownAsync(string userId);  
        Task<Friends?> GetFriendAsync(string userA, string userB);
    }
}
