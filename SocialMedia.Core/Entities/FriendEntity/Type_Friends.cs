using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Entities.FriendEntity
{
    public class Type_Friends
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
        public List<Friends>? friends { get; set; }
    }
}
