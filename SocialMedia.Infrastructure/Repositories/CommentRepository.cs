using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Infrastructure.Data;


namespace SocialMedia.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.comments.FirstOrDefaultAsync(p => p.ID == id);
        }
        public async Task<List<Comment>?> GetCommentByPostIdAsync(int id)
        {
            return await _context.comments.Where(c => c.PostId == id).ToListAsync();
        }

        public async Task<Comment?> AddCommentAsync(Comment comment)
        {
            _context.comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateCommentAsync(Comment comment)
        {
            _context.comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.comments.FindAsync(id);
            if(comment is null)
                return false;
            _context.comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
