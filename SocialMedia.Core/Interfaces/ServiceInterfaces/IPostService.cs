using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface IPostService
    {
        Task<Post?> GetPostByIdAsync(int Id);
        Task<RetrivePostDTO?> AddPostAsync(CreatePostDTO dto);
        Task<RetrivePostDTO?> UpdatePostAsync(int Id, CreatePostDTO dto);
        Task<bool> DeletePostAsync(int Id);
        //Task<IEnumerable<RetrivePostDTO>?> GetPostsByUserIdAsync(string userId);
        Task<IEnumerable<RetrivePostDTO>?> GetRecentPostsAsync(int page, int pageSize);
    }
}
