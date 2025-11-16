using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.PostEntity
{
    public class PostImage
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
