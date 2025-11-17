using AutoMapper;
using SocialMedia.Core.DTO.Comment;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO.Comment;
using SocialMedia.Core.Entities.PostEntity;

namespace SocialMedia.Core.Mapping
{
    public class PostMapping : Profile
    {
        public PostMapping() 
        {
            CreateMap<Post, CreatePostDTO>().ReverseMap();
            CreateMap<Post, RetrivePostDTO>().ReverseMap();
            CreateMap<PostCategory, PostCategoryDTO>().ReverseMap();
            CreateMap<PostCategory, RetriveCategoryDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Comment, RetriveCommentDTO>().ReverseMap();
            CreateMap<PostImage, PostImageDTO>().ReverseMap();
            CreateMap<Like, LikeDTO>().ReverseMap();
        }
    }
}
