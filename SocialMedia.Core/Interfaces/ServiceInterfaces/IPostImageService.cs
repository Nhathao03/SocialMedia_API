using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface IPostImageService
    {
        Task<PostImage?> GetPostImageByIdAsync(int Id);
        Task AddPostImageAsync(CreatePostDTO dto, int postId);
        Task UpdatePostImageAsync(PostImageDTO dto);
        Task<bool> DeletePostImageAsync(int Id);
        //Task<IEnumerable<PostImage>?> GetPostImagesByUserIdAsync(string userId);
        Task<IEnumerable<PostImage>?> GetPostImagesByPostIdAsync(int postId);
    }
}
