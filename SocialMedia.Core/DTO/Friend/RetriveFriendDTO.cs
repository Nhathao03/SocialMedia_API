using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTO.Friend
{
    public class RetriveFriendDTO
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string FriendId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Type_FriendsID { get; set; }
    }
}
