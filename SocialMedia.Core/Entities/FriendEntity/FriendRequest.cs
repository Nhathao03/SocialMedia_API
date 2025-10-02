using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.FriendEntity
{
    public class FriendRequest
    {
        [Key]
        public int ID { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public int status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
