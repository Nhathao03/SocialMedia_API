using AutoMapper;
using SocialMedia.Core.Entities.DTO.RoleCheck;
using SocialMedia.Core.Entities.RoleEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Mapping
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<RoleCheck, RoleCheckDTO>().ReverseMap();
        }
    }
}
