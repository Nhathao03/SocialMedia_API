using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTO.Comment
{
    public class RetriveCommentDTO
    {
        public int ID { get; set; }
        public string? Image { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
    }
}
