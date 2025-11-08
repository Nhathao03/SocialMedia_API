using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Entities.UserEntity
{
    public class UserFollowes
    {
        public int Id { get; set; } 
        public string FollowerId { get; set; }
        public string FollowingId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
