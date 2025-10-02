using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface IPostImageService
    {
        Task<IEnumerable<PostImage>> GetAllPostImageAsync();
        Task<PostImage> GetPostImageByIdAsync(int id);
        Task AddPostImageAsync(PostDTO postDTO, int PostId);
        Task UpdatePostImageAsync(PostImage postimage);
        Task DeletePostImageAsync(int id);
        Task<List<PostImage>> GetAllPostImagesByUserIDAsync(string userId);
        Task<List<PostImage>> GetPostImagesByPostIDAsync(int postId);
    }
}
