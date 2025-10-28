using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.DTO.Friend
{
    public class FriendRequestDTO
    {
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public int status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
