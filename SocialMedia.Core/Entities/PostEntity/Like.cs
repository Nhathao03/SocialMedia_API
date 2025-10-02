using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.PostEntity
{
    public class Like
    {
        [Key]
        public int ID { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public int? Likes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Post Post { get; set; }
        public User user { get; set; }
    }
}
