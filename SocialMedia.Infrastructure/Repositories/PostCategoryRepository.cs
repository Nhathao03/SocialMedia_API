using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.DTO.Post;
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

        public async Task<List<PostCategory>> GetAllPostCategoryAsync()
        {
            return await _context.post_category.ToListAsync();
        }

        public async Task<PostCategory?> GetPostCategoryByIdAsync(int id)
        {
            return await _context.post_category.FindAsync(id);
        }

        public async Task<PostCategory?> AddPostCategoryAsync(PostCategory post)
        {
            await _context.post_category.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<PostCategory?> UpdatePostCategoryAsync(PostCategory post)
        {
            _context.post_category.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePostCategoryAsync(int id)
        {
            var post = await _context.post_category.FindAsync(id);
            if (post is null)
                return false;
            _context.post_category.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
