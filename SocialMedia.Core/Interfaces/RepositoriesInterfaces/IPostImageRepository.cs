using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IPostImageRepository
    {
        Task<IEnumerable<PostImage>> GetAllPostImage();
        Task<PostImage> GetPostImageById(int id);
        Task AddPostImage(List<PostImage> post);
        Task UpdatePostImage(PostImage post);
        Task DeletePostImage(int id);
        Task<List<PostImage>> GetAllPostImagesByUserID(string userId);
        Task<List<PostImage>> GetPostImagesByPostID(int postId);
    }
}
