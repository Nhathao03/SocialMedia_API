using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface IPostCategoryService
    {
        Task<List<PostCategory>?> GetAllPostCategoryAsync();
        Task<PostCategory?> GetPostCategoryByIdAsync(int id);
        Task<RetriveCategoryDTO?> AddPostCategoryAsync(PostCategoryDTO post);
        Task<RetriveCategoryDTO?> UpdatePostCategoryAsync(int id, PostCategoryDTO post);
        Task<bool> DeletePostCategoryAsync(int id);
    }
}
