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

        public async Task<PostImage?> GetPostImageByIdAsync(int Id)
        {
            return await _context.PostImages.FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task AddPostImageAsync(List<PostImage> postImages)
        {
            _context.PostImages.AddRange(postImages);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostImageAsync(PostImage postImage)
        {
            _context.PostImages.Update(postImage);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePostImageAsync(int Id)
        {
            var postImage = await _context.PostImages.FindAsync(Id);
            if(postImage is null)
                return false;
            _context.PostImages.Remove(postImage);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<IEnumerable<PostImage>?> GetPostImagesByUserIdAsync(string userId)
        //{
        //    return await _context.post_image.Where(pi => _context.posts.Where(p => p.UserId == userId)
        //        .Select(p => p.Id)
        //        .Contains(pi.PostId))
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<PostImage>?> GetPostImagesByPostIdAsync(int postId)
        {
            return await _context.PostImages.Where(pi => pi.PostId == postId).ToListAsync();
        }
    }
}
