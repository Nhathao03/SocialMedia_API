using AutoMapper;
using SocialMedia.Core.DTO.Notification;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Mapping
{
    public class NotificationMapping : Profile
    {
        public NotificationMapping() 
        {
            CreateMap<Notification, NotificationCreateDTO>().ReverseMap();
            CreateMap<Notification, RetriveNotificationDTO>().ReverseMap();
        }
    }
}
