using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTO.Post
{
    public class RetriveLikeDTO
    {
        public string? userId {  get; set; }
        public int postId { get; set; }
    }
}
