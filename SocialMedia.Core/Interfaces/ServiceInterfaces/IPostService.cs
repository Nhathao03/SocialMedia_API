using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<Post> AddPostAsync(PostDTO postDTO);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
        Task<IEnumerable<PostDTO>> GetPostsByUserIDAsync(string userID);
        Task<IEnumerable<PostDTO>> GetRecentPostsAsync(int page, int pageSize);
    }
}
