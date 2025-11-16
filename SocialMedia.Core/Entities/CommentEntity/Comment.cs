using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.PostEntity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.CommentEntity
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
