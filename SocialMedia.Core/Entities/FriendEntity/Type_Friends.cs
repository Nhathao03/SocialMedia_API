using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.FriendEntity
{
    public class Type_Friends
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<Friends>? friends { get; set; } = new List<Friends>();
    }
}
