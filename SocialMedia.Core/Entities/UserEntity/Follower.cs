using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.UserEntity
{
    public class Follower
    {
        [Key]
        public int Id { get; set; }
        public string userID { get; set; }
        public string userFollowerID { get; set; }

    }
}
