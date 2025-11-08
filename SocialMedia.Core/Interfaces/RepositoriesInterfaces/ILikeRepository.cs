using SocialMedia.Core.Entities.DTO;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface ILikeRepository
    {
        Task<Like?> GetLikeAsync(Like like);
        Task AddReactionAsync(Like like);
        Task<bool> RemoveReactionAsync(Like like);
        Task<bool> ToggleReactionAsync(Like like);
        Task<int> GetReactionCountAsync(int entityId, EntityTypeEnum entity);
        Task<bool> HasUserReactionAsync(Like like);
        Task<List<string?>> GetUsersReactionAsync(int entityId, EntityTypeEnum entity);
    }
}
