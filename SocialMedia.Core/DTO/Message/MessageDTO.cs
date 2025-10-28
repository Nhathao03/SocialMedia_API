namespace SocialMedia.Core.DTO.Message
{
    public class MessageDTO
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
