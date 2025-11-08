using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.PostEntity
{
    public class Post
    {
        [Key]
        public int ID { get; set; }
        public string? Content { get; set; }
        public int? Views { get; set; }
        public int? Share { get; set; }
        public List<PostImage>? PostImages { get; set; }
        public int PostCategoryID { get; set; }
        public PostCategory PostCategory { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Comment>? Comments { get; set; }
    }
}
