using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.FriendEntity
{
    public class Friends
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string FriendId { get; set; }
        public User Friend { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public int Type_FriendsId { get; set; }
        public Type_Friends? Type_Friends { get; set; }
    }
}
