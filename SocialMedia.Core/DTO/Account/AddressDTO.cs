using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTO.Account
{
    public class AddressDTO
    {
        public string? name { get; set; }
        public string? slug { get; set; }
        public string? type { get; set; }
        public string? name_with_type { get; set; }
    }
}
