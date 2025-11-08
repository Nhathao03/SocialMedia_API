using SocialMedia.Core.DTO.Notification;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<RetriveNotificationDTO>?> GetNotificationsByUserIdAsync(string userId);
        Task<IEnumerable<RetriveNotificationDTO>?> GetUnreadNotificationsAsync(string userId);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(string userId);
        Task<RetriveNotificationDTO?> CreateNotificationAsync(NotificationCreateDTO dto);
    }
}
