using SocialMedia.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using AutoMapper;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostService> _logger;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork,
            ILogger<PostService> logger,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Post?> GetPostByIdAsync(int Id)
        {
            return await _unitOfWork.PostRepository.GetPostByIdAsync(Id);
        }
        public async Task<RetrivePostDTO?> AddPostAsync(CreatePostDTO dto)
        {
            _logger.LogInformation("Adding a new post for user {UserId}", dto.UserId);
            if(dto is null)
                throw new ArgumentNullException(nameof(CreatePostDTO), "Post data is required.");
            var post = _mapper.Map<Post>(dto);
            var result = await _unitOfWork.PostRepository.AddPostAsync(post);
            _logger.LogInformation("Post added with Id {PostId}", result?.Id);
            return _mapper.Map<RetrivePostDTO>(result);
        }

        public async Task<RetrivePostDTO?> UpdatePostAsync(int Id, CreatePostDTO dto)
        {
            _logger.LogInformation("Updating post with Id {PostId}", Id);
            var existingPost = await _unitOfWork.PostRepository.GetPostByIdAsync(Id);
            if (existingPost is null)
            {
                throw new KeyNotFoundException($"Post with Id {Id} not exits.");
            }
            var post = _mapper.Map(dto, existingPost);
            var result =  await _unitOfWork.PostRepository.UpdatePostAsync(post);
            _logger.LogInformation("Post updated with Id {PostId}", result?.Id);
            return _mapper.Map<RetrivePostDTO>(result);
        }

        public async Task<bool> DeletePostAsync(int Id)
        {
            _logger.LogInformation("Deleting post with Id {PostId}", Id);
            var existingPost = await _unitOfWork.PostRepository.GetPostByIdAsync(Id);   
            if (existingPost is null)
            {
                throw new KeyNotFoundException($"Post with Id {Id} not exits.");
            }
            var result = await _unitOfWork.PostRepository.DeletePost(Id);
            _logger.LogInformation("Post deleted with Id {PostId}", Id);
            return result;
        }

        //public async Task<IEnumerable<RetrivePostDTO>?> GetPostsByUserIdAsync(string userId)
        //{
        //    return _mapper.Map<IEnumerable<RetrivePostDTO>?>(
        //        await _unitOfWork.PostRepository.GetPostsByUserIdAsync(userId));
        //}

        public async Task<IEnumerable<RetrivePostDTO>?> GetRecentPostsAsync(int page, int pageSize)
        {
            return _mapper.Map<IEnumerable<RetrivePostDTO>?>(
                await _unitOfWork.PostRepository.GetRecentPostsAsync(page, pageSize));
        }
    }
}
