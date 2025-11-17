using AutoMapper;
using SocialMedia.Core.DTO.Role;
using SocialMedia.Core.Entities.DTO.Role;
using SocialMedia.Core.Entities.DTO.RoleCheck;
using SocialMedia.Core.Entities.RoleEntity;

namespace SocialMedia.Core.Mapping
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<RoleCheck, RoleCheckDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Role, RetriveRoleDTO>().ReverseMap();
        }
    }
}
