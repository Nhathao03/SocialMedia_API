using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTO.Notification
{
    public class RetriveNotificationDTO
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string UserId { get; set; } 
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
