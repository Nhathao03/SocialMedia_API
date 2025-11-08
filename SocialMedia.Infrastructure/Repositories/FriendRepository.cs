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

        public async Task<Friends?> GetFriendByIdAsync(int id)
        {
            return await _context.friends.FirstOrDefaultAsync(p => p.ID == id);
        }
        public async Task<Friends?> AddFriendAsync(Friends friends)
        {
            _context.friends.Add(friends);
            await _context.SaveChangesAsync();
            return friends;
        }

        public async Task<Friends?> UpdateFriendAsync(Friends friends)
        {
            _context.friends.Update(friends);
            await _context.SaveChangesAsync();
            return friends;
        }

        public async Task<bool> DeleteFriendAsync(int id)
        {
            var friend = await _context.friends.FindAsync(id);
            if (friend is null)
                return false;
            _context.friends.Remove(friend);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Friends>?> GetFriendRecentlyAddedAsync(string userID)
        {
            var threeDaysAgo = DateTime.UtcNow.AddDays(-3);
            return await _context.friends
                .Where(f => f.UserID == userID && f.CreatedDate >= threeDaysAgo || f.FriendID == userID && f.CreatedDate >= threeDaysAgo)
                .OrderBy(m => m.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Friends>?> GetFriendOfEachUserAsync(string userID)
        {
            var userFriends = await _context.friends
                .Where(f => f.UserID == userID || f.FriendID == userID)
                .ToListAsync();
            var friendIds = userFriends
                .Select(f => f.UserID == userID ? f.FriendID : f.UserID)
                .Distinct()
                .ToList();
            var result = new List<Friends>();
            foreach (var friendId in friendIds)
            {
                var friendRecord = userFriends
                    .FirstOrDefault(f => (f.UserID == userID && f.FriendID == friendId) || (f.UserID == friendId && f.FriendID == userID));
                if (friendRecord != null)
                {
                    result.Add(friendRecord);
                }
            }
            return result;
        }

        public async Task<List<Friends>?> GetFriendBaseOnHomeTownAsync(string userId)
        {
            var currentUser = await _context.users.FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser == null) return null;

            var usersWithSameAddress = await _context.users
                .Where(u => u.addressID == currentUser.addressID && u.Id != userId)
                .ToListAsync();

            if (usersWithSameAddress == null || !usersWithSameAddress.Any()) return new List<Friends>();

            var result = new List<Friends>();

            foreach (var otherUser in usersWithSameAddress)
            {
                var friend = await _context.friends
                    .FirstOrDefaultAsync(f =>
                        (f.UserID == userId && f.FriendID == otherUser.Id) ||
                        (f.UserID == otherUser.Id && f.FriendID == userId));

                if (friend != null)
                {
                    result.Add(friend);
                }
            }

            return result;
        }

        public async Task<Friends?> GetFriendAsync (string userA, string userB)
        {
            return await _context.friends.FirstOrDefaultAsync(f => (f.UserID == userA && f.FriendID == userB) || (f.FriendID == userA && f.UserID == userB));
        }
    }
}
