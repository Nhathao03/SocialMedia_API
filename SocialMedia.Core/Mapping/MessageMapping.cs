using AutoMapper;
using SocialMedia.Core.DTO.Message;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Mapping
{
    public class MessageMapping : Profile
    {
        public MessageMapping()
        {
            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<Message, RetriveMessageDTO>().ReverseMap();
        }
    }
}
