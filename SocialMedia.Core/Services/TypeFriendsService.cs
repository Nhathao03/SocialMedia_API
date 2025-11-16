using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Core.DTO.Friend;

namespace SocialMedia.Core.Services
{
    public class TypeFriendsService : ITypeFriendsService
    {
        private readonly ITypeFriendsRepository _TypeFriendsRepository;

        public TypeFriendsService(ITypeFriendsRepository repository)
        {
            _TypeFriendsRepository = repository;
        }

        public async Task<IEnumerable<Type_Friends>> GetAllTypeFriendsAsync()
        {
            return await _TypeFriendsRepository.GetAllTypeFriends();
        }

        public async Task<Type_Friends> GetTypeFriendsByIdAsync(int Id)
        {
            return await _TypeFriendsRepository.GetTypeFriendsById(Id);
        }

        public async Task AddTypeFriendsAsync(TypeFriendsDTO TypeFriends)
        {
            var data = new Type_Friends
            {
                Type = TypeFriends.Name
            };
            await _TypeFriendsRepository.AddTypeFriends(data);
        }

        public async Task UpdateTypeFriendsAsync(Type_Friends TypeFriends)
        {
            await _TypeFriendsRepository.UpdateTypeFriends(TypeFriends);
        }

        public async Task DeleteTypeFriendsAsync(int Id)
        {
            await _TypeFriendsRepository.DeleteTypeFriends(Id);
        } 
    }
}
