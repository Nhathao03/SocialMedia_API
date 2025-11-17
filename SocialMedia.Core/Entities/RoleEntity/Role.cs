using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.RoleEntity
{
    public class Role
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        // Relationship
        public ICollection<RoleCheck>? RoleChecks { get; set; } = new List<RoleCheck>();
    }
}
