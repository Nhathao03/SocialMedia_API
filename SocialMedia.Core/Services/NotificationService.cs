using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.DTO.Notification;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces.ServiceInterfaces;

namespace SocialMedia.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RetriveNotificationDTO>?> GetNotificationsByUserIdAsync(string userId)
        {
            _logger.LogInformation("Retrieving notifications for user {userId}", userId);
            if(userId == null)
                throw new ArgumentNullException(nameof(userId), "User Id cannot be null.");
            var notifications = await _unitOfWork.NotificationRepository.GetByUserIdAsync(userId);
            _logger.LogDebug("Retrieved {Count} notifications for user {userId}", notifications?.Count(), userId);
            return _mapper.Map<IEnumerable<RetriveNotificationDTO>>(notifications?.OrderByDescending(n => n.CreatedAt));
        }

        public async Task<IEnumerable<RetriveNotificationDTO>?> GetUnreadNotificationsAsync(string userId)
        {
            _logger.LogInformation("Retrieving unread notifications for user {userId}", userId);
            if(userId is null)
                throw new ArgumentNullException(nameof(userId), "User Id cannot be null.");
            var notifications = await _unitOfWork.NotificationRepository.GetUnreadByUserIdAsync(userId);
            _logger.LogDebug("Retrieved {Count} unread notifications for user {userId}", notifications?.Count(), userId);
            return _mapper.Map<IEnumerable<RetriveNotificationDTO>>(notifications?.OrderByDescending(n => n.CreatedAt));
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            _logger.LogInformation("Counting unread notifications for user {userId}", userId);
            if(userId is null)
                throw new ArgumentNullException(nameof(userId), "User Id cannot be null.");
            var notifications = await _unitOfWork.NotificationRepository.GetUnreadByUserIdAsync(userId);
            _logger.LogDebug("User {userId} has {Count} unread notifications", userId, notifications?.Count());
            return notifications.Count();
        }

        public async Task MarkAsReadAsync(int Id)
        {
            _logger.LogInformation("Marking notification {Id} as read", Id);
            await _unitOfWork.NotificationRepository.MarkAsReadAsync(Id);
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            _logger.LogInformation("Marking all notifications as read for user {userId}", userId);
            await _unitOfWork.NotificationRepository.MarkAllAsReadAsync(userId);
        }

        public async Task<RetriveNotificationDTO?> CreateNotificationAsync(NotificationCreateDTO dto)
        {
            _logger.LogInformation("Creating new notification for user {userId}", dto.SenderId);
            if(dto is null)
                throw new ArgumentNullException(nameof(NotificationCreateDTO), "Notification data is required.");
            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Notification content cannot be empty.", nameof(dto.Content));
            var notification = _mapper.Map<Notification>(dto);
            _logger.LogDebug("Notification details: {@notification}", notification);
            await _unitOfWork.NotificationRepository.AddNotificationAsync(notification);
            _logger.LogInformation("Notification created with Id {NotificationId}", notification.Id);
            return _mapper.Map<RetriveNotificationDTO>(notification);
        }
    }
}
