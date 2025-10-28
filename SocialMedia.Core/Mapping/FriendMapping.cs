using AutoMapper;
using SocialMedia.Core.DTO.Friend;
using SocialMedia.Core.DTO.Message;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.FriendEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Mapping
{
    public class FriendMapping : Profile
    {
        public FriendMapping()
        {
            CreateMap<Friends, FriendDTO>().ReverseMap();
            CreateMap<Friends, RetriveFriendDTO>().ReverseMap();
            CreateMap<FriendRequest, FriendRequestDTO>().ReverseMap();
            CreateMap<FriendRequest, RetriveFriendRequestDTO>().ReverseMap();
        }
    }
}
