using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IPostImageRepository
    {
        Task<PostImage?> GetPostImageByIdAsync(int id);
        Task AddPostImageAsync(List<PostImage> post);
        Task UpdatePostImageAsync(PostImage post);
        Task<bool> DeletePostImageAsync(int id);
        //Task<IEnumerable<PostImage>?> GetPostImagesByUserIdAsync(string userId);
        Task<IEnumerable<PostImage>?> GetPostImagesByPostIdAsync(int postId);
    }
}
