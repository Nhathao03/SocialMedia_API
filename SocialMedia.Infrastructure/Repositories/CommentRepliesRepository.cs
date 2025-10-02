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

        public async Task<IEnumerable<CommentReplies>> GetAllCommentReplies()
        {
            return await _context.commentReplies.ToListAsync();
        }

        public async Task<CommentReplies> GetCommentRepliesById(int id)
        {
            return await _context.commentReplies.FindAsync(id);
        }

        public async Task<int> AddNewCommentReplies(CommentReplies commentReplies)
        {
            var data = _context.commentReplies.Add(commentReplies);
            await _context.SaveChangesAsync();
            return data.Entity.Id;
        }

        public async Task UpdateCommentReplies(CommentReplies commentReplies)
        {
            _context.commentReplies.Update(commentReplies);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentReplies(int id)
        {
            var commentReplies = await _context.commentReplies.FindAsync(id);
            if (commentReplies != null)
            {
                _context.commentReplies.Remove(commentReplies);
                await _context.SaveChangesAsync();
            }
        }
    }
}
