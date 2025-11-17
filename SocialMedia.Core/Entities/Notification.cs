using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public User? Sender { get; set; }
        public string? ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public TargetTypeEnum TargetType { get; set; }
        public int TargetId { get; set; } 
        public string Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum NotificationTypeEnum
    {
        LikePost = 0,
        LikeComment = 1,
        LikeReply = 2,
        CommentPost = 3,
        ReplyComment = 4,
        TagInPost = 5,
        TagInComment = 6,
        Follow = 7,
        FriendRequest = 8,
        FriendRequestAccepted = 9,
        Message = 10,
        Mention = 11,
        StoryReaction = 12,
        StoryReply = 13,
        SystemAlert = 14,
        ReportUpdated = 15
    }

    public enum TargetTypeEnum
    {
        Post = 0,
        Comment = 1,
        Reply = 2,
        UserProfile = 3,
        MessageThread = 4,
        Story = 5,
        SystemPage = 6,
        Report = 7
    }
}
