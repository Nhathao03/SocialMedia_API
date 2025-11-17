using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities
{
    public class Message
	{
		[Key]
		public int Id { get; set; }
		public string SenderId { get; set; }
		public User Sender { get; set; }
        public string ReceiverId { get; set; }
		public User Receiver { get; set; }
        public string Content { get; set; }
		public string? Url { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }
    }
}
