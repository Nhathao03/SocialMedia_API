using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.PostEntity
{
    public class PostCategory
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
