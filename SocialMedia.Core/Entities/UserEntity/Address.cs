using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.UserEntity
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string type { get; set; }
        public string name_with_type { get; set; }
        public ICollection<User> User { get; set; } = new List<User>();
    }
}
