using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.DTO.Post
{
    public class RetrivePostDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Content { get; set; }
        public int? Views { get; set; }
        public int? Share { get; set; }
        public List<PostImage>? PostImages { get; set; }
        public PostCategory? PostCategory { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User? user { get; set; }

        public List<Like>? Likes { get; set; }

        // Navigation properties
        public List<Entities.CommentEntity.Comment>? Comments { get; set; }
        public List<Entities.Report>? Reports { get; set; }
    }
}
