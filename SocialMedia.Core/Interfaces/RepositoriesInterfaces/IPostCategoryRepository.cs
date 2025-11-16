using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IPostCategoryRepository
    {
        Task<List<PostCategory>?> GetAllPostCategoryAsync();
        Task<PostCategory?> GetPostCategoryByIdAsync(int Id);
        Task<PostCategory?> AddPostCategoryAsync(PostCategory post);
        Task<PostCategory?> UpdatePostCategoryAsync(PostCategory post);
        Task<bool> DeletePostCategoryAsync(int Id);
    }
}
