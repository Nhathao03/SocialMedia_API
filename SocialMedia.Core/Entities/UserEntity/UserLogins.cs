using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.Entity
{
    public class UserLogins
    {
        [Key]
        public int Id { get; set; }
        public string LoginProvIder { get; set; }
        public string ProvIderKey { get; set; }
        public string ProvIderDisplayName { get; set; }
        public string UserId { get; set; }
    }
}
