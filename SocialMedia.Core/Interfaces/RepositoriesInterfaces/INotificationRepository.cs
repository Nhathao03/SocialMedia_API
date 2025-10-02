using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface INotificationRepository
    {
        Task CreateNotification(Notification model);
        Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId);
        Task MarkAsRead(int notificationId);
        Task DeleteNotification(int notificationId);
    }
}
