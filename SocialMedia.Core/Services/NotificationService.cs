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
            var notifications = await _unitOfWork.NotificationRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RetriveNotificationDTO>>(notifications?.OrderByDescending(n => n.CreatedAt));
        }

        public async Task<IEnumerable<RetriveNotificationDTO>?> GetUnreadNotificationsAsync(string userId)
        {
            _logger.LogInformation("Retrieving unread notifications for user {userId}", userId);
            var notifications = await _unitOfWork.NotificationRepository.GetUnreadByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RetriveNotificationDTO>>(notifications?.OrderByDescending(n => n.CreatedAt));
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            _logger.LogInformation("Counting unread notifications for user {userId}", userId);
            var notifications = await _unitOfWork.NotificationRepository.GetUnreadByUserIdAsync(userId);
            return notifications.Count();
        }

        public async Task MarkAsReadAsync(int id)
        {
            _logger.LogInformation("Marking notification {id} as read", id);
            await _unitOfWork.NotificationRepository.MarkAsReadAsync(id);
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            _logger.LogInformation("Marking all notifications as read for user {userId}", userId);
            await _unitOfWork.NotificationRepository.MarkAllAsReadAsync(userId);
        }

        public async Task<RetriveNotificationDTO?> CreateNotificationAsync(NotificationCreateDTO dto)
        {
            _logger.LogInformation("Creating new notification for user {userId}", dto.UserId);
            var notification = _mapper.Map<Notification>(dto);
            _logger.LogDebug("Notification details: {@notification}", notification);
            await _unitOfWork.NotificationRepository.AddNotificationAsync(notification);
            return _mapper.Map<RetriveNotificationDTO>(notification);
        }
    }
}
