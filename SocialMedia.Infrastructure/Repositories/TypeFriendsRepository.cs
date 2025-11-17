using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class TypeFriendsRepository : ITypeFriendsRepository
    {
        private readonly AppDbContext _context;

        public TypeFriendsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Type_Friends>> GetAllTypeFriends()
        {
            return await _context.TypeFriends.ToListAsync();
        }

        public async Task<Type_Friends> GetTypeFriendsById(int Id)
        {
            return await _context.TypeFriends.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task UpdateTypeFriends(Type_Friends TypeFriends)
        {
            _context.TypeFriends.Update(TypeFriends);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTypeFriends(int Id)
        {
            var TypeFriends = await _context.TypeFriends.FindAsync(Id);
            if (TypeFriends != null)
            {
                _context.TypeFriends.Remove(TypeFriends);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTypeFriends (Type_Friends model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
        }

    }
}
