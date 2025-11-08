using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostImageRepository : IPostImageRepository
    {
        private readonly AppDbContext _context;

        public PostImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PostImage?> GetPostImageByIdAsync(int id)
        {
            return await _context.post_image.FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task AddPostImageAsync(List<PostImage> postImages)
        {
            _context.post_image.AddRange(postImages);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostImageAsync(PostImage postImage)
        {
            _context.post_image.Update(postImage);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePostImageAsync(int id)
        {
            var postImage = await _context.post_image.FindAsync(id);
            if(postImage is null)
                return false;
            _context.post_image.Remove(postImage);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<IEnumerable<PostImage>?> GetPostImagesByUserIdAsync(string userId)
        //{
        //    return await _context.post_image.Where(pi => _context.posts.Where(p => p.UserID == userId)
        //        .Select(p => p.ID)
        //        .Contains(pi.PostId))
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<PostImage>?> GetPostImagesByPostIdAsync(int postId)
        {
            return await _context.post_image.Where(pi => pi.PostId == postId).ToListAsync();
        }
    }
}
