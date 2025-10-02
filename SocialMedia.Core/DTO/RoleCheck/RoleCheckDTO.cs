using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.DTO.RoleCheck
{
    public class RoleCheckDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string RoleId { get; set; }
    }
}
