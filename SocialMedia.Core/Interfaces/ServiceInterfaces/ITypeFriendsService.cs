using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Core.Services
{
    public interface ITypeFriendsService
    {
        Task<IEnumerable<Type_Friends>> GetAllTypeFriendsAsync();
        Task<Type_Friends> GetTypeFriendsByIdAsync(int id);
        Task AddTypeFriendsAsync(TypeFriendsDTO TypeFriends);
        Task UpdateTypeFriendsAsync(Type_Friends TypeFriends);
        Task DeleteTypeFriendsAsync(int id);
    }
}
