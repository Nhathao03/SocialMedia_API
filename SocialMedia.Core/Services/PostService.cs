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

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _unitOfWork.PostRepository.GetPostByIdAsync(id);
        }
        public async Task<RetrivePostDTO?> AddPostAsync(PostDTO dto)
        {
            _logger.LogInformation("Adding a new post for user {UserID}", dto.UserID);
            if(dto is null)
                throw new ArgumentNullException(nameof(PostDTO), "Post data is required.");
            var post = _mapper.Map<Post>(dto);
            var result = await _unitOfWork.PostRepository.AddPostAsync(post);
            _logger.LogInformation("Post added with ID {PostID}", result?.ID);
            return _mapper.Map<RetrivePostDTO>(result);
        }

        public async Task<RetrivePostDTO?> UpdatePostAsync(int id, PostDTO dto)
        {
            _logger.LogInformation("Updating post with ID {PostID}", id);
            var existingPost = await _unitOfWork.PostRepository.GetPostByIdAsync(id);
            if (existingPost is null)
            {
                throw new KeyNotFoundException($"Post with Id {id} not exits.");
            }
            var post = _mapper.Map(dto, existingPost);
            var result =  await _unitOfWork.PostRepository.UpdatePostAsync(post);
            _logger.LogInformation("Post updated with ID {PostID}", result?.ID);
            return _mapper.Map<RetrivePostDTO>(result);
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            _logger.LogInformation("Deleting post with ID {PostID}", id);
            var existingPost = await _unitOfWork.PostRepository.GetPostByIdAsync(id);   
            if (existingPost is null)
            {
                throw new KeyNotFoundException($"Post with Id {id} not exits.");
            }
            var result = await _unitOfWork.PostRepository.DeletePost(id);
            _logger.LogInformation("Post deleted with ID {PostID}", id);
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
