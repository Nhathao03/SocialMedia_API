using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.PostEntity
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string? Content { get; set; }
        public int Views { get; set; }
        public int Share { get; set; }
        public int PostCategoryId { get; set; }
        public PostCategory PostCategory { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public ICollection<PostImage>? PostImages { get; set; } = new List<PostImage>();
    }
}
