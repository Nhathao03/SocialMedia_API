using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Like?> GetLikeAsync(Like like)
        {
            return await _context.likes
                .FirstOrDefaultAsync(l => l.UserId == like.UserId && l.EntityId == like.EntityId && l.EntityType == like.EntityType);
        }
        public async Task AddReactionAsync(Like like)
        {
            _context.likes.Add(like);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveReactionAsync(Like like)
        {
            var existing = await _context.likes
           .FirstOrDefaultAsync(l => l.UserId == like.UserId && l.EntityId == like.EntityId && l.EntityType == like.EntityType);
            if (existing == null)
            {
                return false; // Not found
            }
            _context.likes.Remove(existing);
            await _context.SaveChangesAsync();
            return true; // Removed
        }

        public async Task<bool> ToggleReactionAsync(Like like)
        {
            var existing = await _context.likes
            .FirstOrDefaultAsync(l => l.UserId == like.UserId && l.EntityId == like.EntityId && l.EntityType == like.EntityType);

            if (existing != null)
            {
                _context.likes.Remove(existing);
                await _context.SaveChangesAsync();
                return false; // Unliked
            }

            _context.likes.Add(new Like
            {
                UserId = like.UserId,
                EntityId = like.EntityId,
                EntityType = like.EntityType
            });
            await _context.SaveChangesAsync();
            return true; // Liked
        }

        public async Task<int> GetReactionCountAsync(int entityId, EntityTypeEnum entity)
        {
            return await _context.likes.CountAsync(l => l.EntityId == entityId && l.EntityType == entity);
        }

        public async Task<bool> HasUserReactionAsync(Like like)
        {
            return await _context.likes.AnyAsync(l => l.UserId == like.UserId && l.EntityId == like.EntityId && l.EntityType == like.EntityType);
        }

        public async Task<List<string?>> GetUsersReactionAsync(int entityId, EntityTypeEnum entity)
        {
            return await _context.likes
                .Where(l => l.EntityId == entityId && l.EntityType == entity)
                .Select(l => l.UserId)
                .ToListAsync();
        }
    }
}
