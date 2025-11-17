using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface IPostCategoryService
    {
        Task<List<PostCategory>?> GetAllPostCategoryAsync();
        Task<PostCategory?> GetPostCategoryByIdAsync(int Id);
        Task<RetriveCategoryDTO?> AddPostCategoryAsync(PostCategoryDTO post);
        Task<RetriveCategoryDTO?> UpdatePostCategoryAsync(int Id, PostCategoryDTO post);
        Task<bool> DeletePostCategoryAsync(int Id);
    }
}
