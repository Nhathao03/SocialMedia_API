using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostCategoryRepository : IPostCategoryRepository
    {
        private readonly AppDbContext _context;

        public PostCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostCategory>> GetAllPostCategory()
        {
            return await _context.post_category.ToListAsync();
        }

        public async Task<PostCategory> GetPostCategoryById(int id)
        {
            return await _context.post_category.FindAsync(id);
        }

        public async Task AddPostCategory(PostCategory post)
        {
            await _context.post_category.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostCategory(PostCategory post)
        {
            _context.post_category.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostCategory(int id)
        {
            var post = await _context.post_category.FindAsync(id);
            if (post != null)
            {
                _context.post_category.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
