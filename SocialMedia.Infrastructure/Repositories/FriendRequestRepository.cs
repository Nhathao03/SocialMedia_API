using Microsoft.EntityFrameworkCore;
using Social_Media.Helpers;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Infrastructure.Data;
using static Social_Media.Helpers.Constants;
namespace SocialMedia.Infrastructure.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly AppDbContext _context;

        public FriendRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FriendRequest>?> GetAllFriendRequestAsync()
        {
            return await _context.friendRequests.ToListAsync();
        }
        public async Task<FriendRequest?> GetFriendRequestByIdAsync(int id)
        {
            return await _context.friendRequests.FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task<FriendRequest?> GetFriendRequestBetweenUsersAsync(string userA, string userB)
        {
            return await _context.friendRequests
                .FirstOrDefaultAsync(fr =>
                    (fr.SenderID == userA && fr.ReceiverID == userB) ||
                    (fr.SenderID == userB && fr.ReceiverID == userA));
        }

        public async Task<List<FriendRequest>?> GetFriendRequestByUserIdAsync(string id)
        {
            return await _context.friendRequests.Where(f => f.SenderID == id || f.ReceiverID == id).ToListAsync();
        }

        public async Task<FriendRequest?> AddFriendRequestAsync(FriendRequest model)
        {
            _context.friendRequests.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<FriendRequest?> UpdateFriendRequestAsync(FriendRequest model)
        {
            _context.friendRequests.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteFriendRequestAsync(int id)
        {
            var existingfriendRequest = await _context.friendRequests.FindAsync(id);
            if(existingfriendRequest is null)
                return false;
            _context.friendRequests.Remove(existingfriendRequest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FriendRequest>?> GetSentRequestAsync(string userId)
        {
            return await _context.friendRequests.Where(fr => fr.SenderID == userId && fr.status == 1).ToListAsync();
        }

        public async Task<List<FriendRequest>?> GetReceivedRequestAsync(string userId)
        {
            return await _context.friendRequests.Where(fr => fr.ReceiverID == userId && fr.status == 1).ToListAsync();
        }
    }
}
