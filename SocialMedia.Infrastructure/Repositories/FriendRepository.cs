using Microsoft.EntityFrameworkCore;
using Social_Media.Helpers;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Infrastructure.Data;
using static Social_Media.Helpers.Constants;
namespace SocialMedia.Infrastructure.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly AppDbContext _context;

        public FriendRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Friends?> GetFriendByIdAsync(int Id)
        {
            return await _context.Friends.FirstOrDefaultAsync(p => p.Id == Id);
        }
        public async Task<Friends?> AddFriendAsync(Friends friends)
        {
            _context.Friends.Add(friends);
            await _context.SaveChangesAsync();
            return friends;
        }

        public async Task<Friends?> UpdateFriendAsync(Friends friends)
        {
            _context.Friends.Update(friends);
            await _context.SaveChangesAsync();
            return friends;
        }

        public async Task<bool> DeleteFriendAsync(int Id)
        {
            var friend = await _context.Friends.FindAsync(Id);
            if (friend is null)
                return false;
            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Friends>?> GetFriendRecentlyAddedAsync(string userId)
        {
            var threeDaysAgo = DateTime.UtcNow.AddDays(-3);
            return await _context.Friends
                .Where(f => f.UserId == userId && f.CreatedAt >= threeDaysAgo || f.FriendId == userId && f.CreatedAt >= threeDaysAgo)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Friends>?> GetFriendOfEachUserAsync(string userId)
        {
            var userFriends = await _context.Friends
                .Where(f => f.UserId == userId || f.FriendId == userId)
                .ToListAsync();
            var friendIds = userFriends
                .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                .Distinct()
                .ToList();
            var result = new List<Friends>();
            foreach (var friendId in friendIds)
            {
                var friendRecord = userFriends
                    .FirstOrDefault(f => (f.UserId == userId && f.FriendId == friendId) || (f.UserId == friendId && f.FriendId == userId));
                if (friendRecord != null)
                {
                    result.Add(friendRecord);
                }
            }
            return result;
        }

        public async Task<List<Friends>?> GetFriendBaseOnHomeTownAsync(string userId)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser == null) return null;

            var usersWithSameAddress = await _context.Users
                .Where(u => u.AddressId == currentUser.AddressId && u.Id != userId)
                .ToListAsync();

            if (usersWithSameAddress == null || !usersWithSameAddress.Any()) return new List<Friends>();

            var result = new List<Friends>();

            foreach (var otherUser in usersWithSameAddress)
            {
                var friend = await _context.Friends
                    .FirstOrDefaultAsync(f =>
                        (f.UserId == userId && f.FriendId == otherUser.Id) ||
                        (f.UserId == otherUser.Id && f.FriendId == userId));

                if (friend != null)
                {
                    result.Add(friend);
                }
            }

            return result;
        }

        public async Task<Friends?> GetFriendAsync (string userA, string userB)
        {
            return await _context.Friends.FirstOrDefaultAsync(f => (f.UserId == userA && f.FriendId == userB) || (f.FriendId == userA && f.UserId == userB));
        }
    }
}
