using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>?> GetByUserIdAsync(string userId);
        Task<IEnumerable<Notification>?> GetUnreadByUserIdAsync(string userId);
        Task<Notification?> GetByIdAsync(int id);
        Task<Notification?> AddNotificationAsync(Notification notification);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(string userId);
    }
}
