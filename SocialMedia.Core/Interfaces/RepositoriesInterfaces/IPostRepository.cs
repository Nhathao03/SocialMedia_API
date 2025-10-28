using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPost();
        Task<Post> GetPostById(int id);
        Task<Post> AddPost(Post post);
        Task UpdatePost(Post post);
        Task DeletePost(int id);
        Task<IEnumerable<PostDTO>> GetPostsByUserID(string userID);
        Task<IEnumerable<PostDTO>> GetRecentPostsAsync(int page, int pageSize);
    }
}
