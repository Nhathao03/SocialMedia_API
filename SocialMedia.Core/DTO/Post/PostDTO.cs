using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using SocialMedia.Core.Entities.DTO.Comment;

namespace SocialMedia.Core.DTO.Post
{
    public class PostDTO
    {
        public string? UserID { get; set; }
        public string? Content { get; set; }
        public int? Views { get; set; }
        public int? Share { get; set; } = 0;
        public int PostCategoryID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<PostImageDTO>? PostImages { get; set; }
    }

    public class PostImageDTO
    {
        public string? Url { get; set; }
    }
}


