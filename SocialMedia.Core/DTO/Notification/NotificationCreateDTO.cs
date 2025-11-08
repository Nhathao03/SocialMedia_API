using SocialMedia.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.DTO.Notification
{
    public class NotificationCreateDTO
    {
        public string UserId { get; set; }
        public string ReceiverId { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public TargetTypeEnum TargetType { get; set; }
        public int? TargetId { get; set; }
        public string? Content { get; set; }       
    }
}
