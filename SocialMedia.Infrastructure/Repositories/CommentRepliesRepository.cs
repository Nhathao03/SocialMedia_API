using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Core.Interfaces.RepositoriesInterfaces;

namespace SocialMedia.Infrastructure.Repositories
{
    public class CommentRepliesRepository : ICommentRepliesRepository
    {
        private readonly AppDbContext _context;

        public CommentRepliesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CommentReplies?> GetCommentRepliesByIdAsync(int id)
        {
            return await _context.commentReplies.FindAsync(id);
        }

        public async Task<CommentReplies?> AddNewCommentRepliesAsync(CommentReplies commentReplies)
        {
            _context.commentReplies.Add(commentReplies);
            await _context.SaveChangesAsync();
            return commentReplies;
        }

        public async Task<CommentReplies?> UpdateCommentRepliesAsync(CommentReplies commentReplies)
        {
            _context.commentReplies.Update(commentReplies);
            await _context.SaveChangesAsync();
            return commentReplies;
        }

        public async Task<bool> DeleteCommentRepliesAsync(int id)
        {
            var commentReplies = await _context.commentReplies.FindAsync(id);
            if(commentReplies is null)
                return false;
            _context.commentReplies.Remove(commentReplies);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
