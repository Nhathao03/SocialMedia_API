using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetPostByIdAsync(int id);
        Task<Post?> AddPostAsync(Post post);
        Task<Post?> UpdatePostAsync(Post post);
        Task<bool> DeletePost(int id);
        //Task<IEnumerable<RetrivePostDTO>?> GetPostsByUserIdAsync(string userId);
        Task<IEnumerable<Post>?> GetRecentPostsAsync(int page, int pageSize);
    }
}
