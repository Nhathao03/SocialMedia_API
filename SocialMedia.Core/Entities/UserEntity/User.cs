using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.FriendEntity;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Core.Entities.RoleEntity;
using SocialMedia.Core.Entities.UserEntity;
using System;
using System.ComponentModel.DataAnnotations;


namespace SocialMedia.Core.Entities.Entity
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime? Birth { get; set; }
        public string PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; }
        public string? BackgroundProfile { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string NormalizeUsername { get; set; }
        public string NormalizeEmail { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<Post>? Posts { get; set; } = new List<Post>();
        public ICollection<RoleCheck>? RoleChecks { get; set; } = new List<RoleCheck>();
        public ICollection<Like>? Likes { get; set; } = new List<Like>();
        public ICollection<UserFollowes>? UserFollower { get; set; } = new List<UserFollowes>();
        public ICollection<UserFollowes>? UserFollowing { get; set; } = new List<UserFollowes>();
        public ICollection<Notification>? SentNotifications { get; set; } = new List<Notification>();
        public ICollection<Notification>? ReceivedNotifications { get; set; } = new List<Notification>();
        public ICollection<Report>? Reports { get; set; } = new List<Report>();
        public ICollection<Message>? SentMessages { get; set; } = new List<Message>();
        public ICollection<Message>? ReceivedMessages { get; set; } = new List<Message>();
        public ICollection<FriendRequest>? SentFriendRequests { get; set; } = new List<FriendRequest>();
        public ICollection<FriendRequest>? ReceivedFriendRequests { get; set; } = new List<FriendRequest>();
        public ICollection<Friends>? UserFriendsA { get; set; } = new List<Friends>();
        public ICollection<Friends>? UserFriendsB { get; set; } = new List<Friends>();
    }
}
