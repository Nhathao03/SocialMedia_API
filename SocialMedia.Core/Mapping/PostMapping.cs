using AutoMapper;
using SocialMedia.Core.DTO.Comment;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO.Comment;
using SocialMedia.Core.Entities.PostEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Mapping
{
    public class PostMapping : Profile
    {
        public PostMapping() 
        {
            CreateMap<PostCategory, PostCategoryDTO>().ReverseMap();
            CreateMap<PostCategory, RetriveCategoryDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Comment, RetriveCommentDTO>().ReverseMap();
        }
    }
}
