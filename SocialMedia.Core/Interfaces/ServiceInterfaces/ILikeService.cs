using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Services
{
    public interface ILikeService
    {
        Task AddReactionAsync(LikeDTO dto);
        Task<bool> RemoveReactionAsync(LikeDTO dto);
        Task<bool> ToggleReactionAsync(LikeDTO dto);
        Task<int> GetReactionCountAsync(int entityId, EntityTypeEnum entity);
        Task<bool> HasUserReactionAsync(LikeDTO dto);
        Task<List<string?>> GetUsersReactionAsync(int entityId, EntityTypeEnum entity);
    }
}
   