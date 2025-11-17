using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace SocialMedia.Core.Services
{
    public class PostImageService : IPostImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PostImageService> _logger;

        public PostImageService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PostImageService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PostImage?> GetPostImageByIdAsync(int Id)
        {
            _logger.LogInformation("Retrieving PostImage with Id {PostImageId}", Id);
            return await _unitOfWork.PostImageRepository.GetPostImageByIdAsync(Id);
        }

        public async Task AddPostImageAsync(CreatePostDTO dto, int postId)
        {
            _logger.LogInformation("Adding a new PostImage for Post Id {PostId}", postId);
            if(dto is null)
                throw new ArgumentNullException(nameof(PostImageDTO), "PostImage data is required.");
            if(postId <= 0)
                throw new ArgumentException("InvalId Post Id.", nameof(postId));
            var postImages = dto.PostImages.Select(imageUrl => new PostImage
            {
                Url = imageUrl.Url,
                PostId = postId
            }).ToList();
            await _unitOfWork.PostImageRepository.AddPostImageAsync(postImages);
            _logger.LogInformation("Added {Count} PostImages for Post Id {PostId}", postImages.Count, postId);
        }

        public async Task UpdatePostImageAsync(PostImageDTO dto)
        {  
            if(dto is null)
                throw new ArgumentNullException(nameof(PostImageDTO), "PostImage data is required.");
            var postimage = _mapper.Map<PostImage>(dto);
            _logger.LogInformation("PostImage URL: {PostImageUrl}", postimage.Url);
            await  _unitOfWork.PostImageRepository.UpdatePostImageAsync(postimage);
        }

        public async Task<bool> DeletePostImageAsync(int Id)
        {
            _logger.LogInformation("Deleting PostImage with Id {PostImageId}", Id);
            var existingPostImage = await _unitOfWork.PostImageRepository.GetPostImageByIdAsync(Id);
            if (existingPostImage is null)
            {
                _logger.LogWarning("PostImage with Id {PostImageId} not found", Id);
                throw new KeyNotFoundException($"PostImage with Id {Id} not exits.");
            }
            var result = await _unitOfWork.PostImageRepository.DeletePostImageAsync(Id);
            _logger.LogInformation("PostImage with Id {PostImageId} deleted: {Result}", Id, result);
            return result;
        }

        //public async Task<IEnumerable<PostImage>?> GetPostImagesByUserIdAsync(string userId)
        //{
        //    return await _unitOfWork.PostImageRepository.GetPostImagesByUserIdAsync(userId);
        //}

        public async Task<IEnumerable<PostImage>?> GetPostImagesByPostIdAsync(int postId)
        {
            return await _unitOfWork.PostImageRepository.GetPostImagesByPostIdAsync(postId);
        }
    }
}
