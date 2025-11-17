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
            return await _context.PostCategories.ToListAsync();
        }

        public async Task<PostCategory?> GetPostCategoryByIdAsync(int Id)
        {
            return await _context.PostCategories.FindAsync(Id);
        }

        public async Task<PostCategory?> AddPostCategoryAsync(PostCategory post)
        {
            await _context.PostCategories.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<PostCategory?> UpdatePostCategoryAsync(PostCategory post)
        {
            _context.PostCategories.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePostCategoryAsync(int Id)
        {
            var post = await _context.PostCategories.FindAsync(Id);
            if (post is null)
                return false;
            _context.PostCategories.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
