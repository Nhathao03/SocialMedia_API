using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }
        public async Task CreateNotification(NotificationDTO model)
        {
            var notification = new Notification
            {
                UserId = model.UserId,
                Message = model.Message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.CreateNotification(notification);
        }
        public async Task<int> GetNotificationCount(string userId)
        {
            var notifications = await _repository.GetNotificationsByUserId(userId);
            return notifications.Count(n => !n.IsRead);
        }

        public async Task MarkAllAsRead(string userId)
        {
            var notifications = await _repository.GetNotificationsByUserId(userId);
            foreach (var notification in notifications.Where(n => !n.IsRead))
            {
                await _repository.MarkAsRead(notification.Id);
            }
        }

        public async Task DeleteNotification(int notificationId)
        {
            await _repository.DeleteNotification(notificationId);
        }
    }
}
