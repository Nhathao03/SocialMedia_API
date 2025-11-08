using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface IPostImageService
    {
        Task<PostImage?> GetPostImageByIdAsync(int id);
        Task AddPostImageAsync(PostDTO dto, int postId);
        Task UpdatePostImageAsync(PostImageDTO dto);
        Task<bool> DeletePostImageAsync(int id);
        //Task<IEnumerable<PostImage>?> GetPostImagesByUserIdAsync(string userId);
        Task<IEnumerable<PostImage>?> GetPostImagesByPostIdAsync(int postId);
    }
}
