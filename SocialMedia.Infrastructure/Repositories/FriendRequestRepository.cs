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
            return await _context.FriendRequests.ToListAsync();
        }
        public async Task<FriendRequest?> GetFriendRequestByIdAsync(int Id)
        {
            return await _context.FriendRequests.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<FriendRequest?> GetFriendRequestBetweenUsersAsync(string userA, string userB)
        {
            return await _context.FriendRequests
                .FirstOrDefaultAsync(fr =>
                    (fr.SenderId == userA && fr.ReceiverId == userB) ||
                    (fr.SenderId == userB && fr.ReceiverId == userA));
        }

        public async Task<List<FriendRequest>?> GetFriendRequestByUserIdAsync(string Id)
        {
            return await _context.FriendRequests.Where(f => f.SenderId == Id || f.ReceiverId == Id).ToListAsync();
        }

        public async Task<FriendRequest?> AddFriendRequestAsync(FriendRequest model)
        {
            _context.FriendRequests.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<FriendRequest?> UpdateFriendRequestAsync(FriendRequest model)
        {
            _context.FriendRequests.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteFriendRequestAsync(int Id)
        {
            var existingfriendRequest = await _context.FriendRequests.FindAsync(Id);
            if(existingfriendRequest is null)
                return false;
            _context.FriendRequests.Remove(existingfriendRequest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FriendRequest>?> GetSentRequestAsync(string userId)
        {
            return await _context.FriendRequests.Where(fr => fr.SenderId == userId && fr.Status == 1).ToListAsync();
        }

        public async Task<List<FriendRequest>?> GetReceivedRequestAsync(string userId)
        {
            return await _context.FriendRequests.Where(fr => fr.ReceiverId == userId && fr.Status == 1).ToListAsync();
        }
    }
}
