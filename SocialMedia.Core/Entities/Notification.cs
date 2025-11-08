using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ReceiverId { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public TargetTypeEnum TargetType { get; set; }
        public int? TargetId { get; set; } 
        public string? Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationship
        public User User { get; set; }
    }

    public enum NotificationTypeEnum
    {
        LikePost = 1,
        LikeComment = 2,
        LikeReply = 3,
        CommentPost = 4,
        ReplyComment = 5,
        TagInPost = 6,
        TagInComment = 7,
        Follow = 8,
        FriendRequest = 9,
        FriendRequestAccepted = 10,
        Message = 11,
        Mention = 12,
        StoryReaction = 13,
        StoryReply = 14,
        SystemAlert = 15,
        ReportUpdated = 16
    }

    public enum TargetTypeEnum
    {
        Post = 1,
        Comment = 2,
        Reply = 3,
        UserProfile = 4,
        MessageThread = 5,
        Story = 6,
        SystemPage = 7,
        Report = 8
    }
}
