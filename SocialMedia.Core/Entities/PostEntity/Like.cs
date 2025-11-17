using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Entities.PostEntity
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public LikeTypeEnum LikeType { get; set; }
        public EntityTypeEnum EntityType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum LikeTypeEnum
    {
        Like = 0,
        Love = 1,
        Haha = 2,
        Wow = 3,
        Sad = 4,
        Angry = 5,
    }

    public enum EntityTypeEnum
    {
        Post = 0,
        Comment = 1,
        Reply = 2,
    }
}
