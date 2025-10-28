namespace SocialMedia.Core.Entities.DTO.Comment
{
    public class CommentDTO
    {
        public string? Image { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt {  get; set; } = DateTime.Now;
    }
}
