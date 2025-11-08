using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface IPostService
    {
        Task<Post?> GetPostByIdAsync(int id);
        Task<RetrivePostDTO?> AddPostAsync(PostDTO dto);
        Task<RetrivePostDTO?> UpdatePostAsync(int id, PostDTO dto);
        Task<bool> DeletePostAsync(int id);
        //Task<IEnumerable<RetrivePostDTO>?> GetPostsByUserIdAsync(string userID);
        Task<IEnumerable<RetrivePostDTO>?> GetRecentPostsAsync(int page, int pageSize);
    }
}
