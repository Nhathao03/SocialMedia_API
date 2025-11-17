using SocialMedia.Core.Entities.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Entities.UserEntity
{
    public class UserFollowes
    {
        [Key]
        public int Id { get; set; } 
        public string FollowerId { get; set; }
        public User Follower { get; set; }
        public string FollowingId { get; set; }
        public User Following { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
