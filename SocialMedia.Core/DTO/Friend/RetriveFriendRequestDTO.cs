using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTO.Friend
{
    public class RetriveFriendRequestDTO
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int status { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
