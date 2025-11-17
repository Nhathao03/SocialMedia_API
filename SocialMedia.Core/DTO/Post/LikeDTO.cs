using SocialMedia.Core.Entities.PostEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTO.Post
{
    public class LikeDTO
    {
        public int EntityId { get; set; }
        public string? UserId { get; set; }
        public LikeTypeEnum LikeType { get; set; }
        public EntityTypeEnum EntityType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
