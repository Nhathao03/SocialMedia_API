using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.FriendEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface ITypeFriendsRepository
    {
        Task<IEnumerable<Type_Friends>> GetAllTypeFriends();
        Task<Type_Friends> GetTypeFriendsById(int id);
        Task AddTypeFriends(Type_Friends TypeFriends);
        Task UpdateTypeFriends(Type_Friends TypeFriends);
        Task DeleteTypeFriends(int id);
    }
}
