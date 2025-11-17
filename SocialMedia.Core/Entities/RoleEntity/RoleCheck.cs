using SocialMedia.Core.Entities.Entity;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.RoleEntity
{
    public class RoleCheck
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }

    }
}
