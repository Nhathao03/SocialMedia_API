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
        public async Task<Comment?> GetCommentByIdAsync(int Id)
        {
            return await _context.Comments.FirstOrDefaultAsync(p => p.Id == Id);
        }
        public async Task<List<Comment>?> GetCommentByPostIdAsync(int Id)
        {
            return await _context.Comments.Where(c => c.PostId == Id).ToListAsync();
        }

        public async Task<Comment?> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentAsync(int Id)
        {
            var comment = await _context.Comments.FindAsync(Id);
            if(comment is null)
                return false;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
