using AutoMapper;
using SocialMedia.Core.DTO.Account;
using SocialMedia.Core.Entities.DTO.Account;
using SocialMedia.Core.Entities.Entity;
using SocialMedia.Core.Entities.UserEntity;

namespace SocialMedia.Core.Mapping
{
    public class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<User, ProfileDTO>().ReverseMap();
            CreateMap<User, UpdateProfileDTO>().ReverseMap();
            CreateMap<User, UpdateBackgroundDTO>().ReverseMap();
            CreateMap<User, UpdateProfileDTO>().ReverseMap();
            CreateMap<User, UpdateContactDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Address, RetriveAddressDTO>().ReverseMap();
        }
    }
}
