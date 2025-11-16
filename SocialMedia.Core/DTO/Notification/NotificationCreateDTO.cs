using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.DTO.Notification
{
    public class NotificationCreateDTO
    {
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public TargetTypeEnum TargetType { get; set; }
        [Required]
        public int? TargetId { get; set; }
        [Required]
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
