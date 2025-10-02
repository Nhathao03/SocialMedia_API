using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Core.Services
{
    public interface INotificationService
    {
        Task<int> GetNotificationCount(string userId);
        Task MarkAllAsRead(string userId);
        Task CreateNotification(NotificationDTO model);
        Task DeleteNotification(int notificationId);

    }
}
