using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.DTO.Post;
using SocialMedia.Core.Entities.PostEntity;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Core.Services
{
    public class PostCategoryService : IPostCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PostCategoryService> _logger;

        public PostCategoryService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PostCategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<PostCategory>?> GetAllPostCategoryAsync()
        {
            return await _unitOfWork.PostCategoryRepository.GetAllPostCategoryAsync();
        }

        public async Task<PostCategory?> GetPostCategoryByIdAsync(int Id)
        {
            return await _unitOfWork.PostCategoryRepository.GetPostCategoryByIdAsync(Id);
        }

        public async Task<RetriveCategoryDTO?> AddPostCategoryAsync(PostCategoryDTO dto)
        {
            _logger.LogInformation("Adding a new post category with {CategoryName}", dto?.name);
            if(dto is null)
                throw new ArgumentNullException(nameof(PostCategoryDTO), "Post category data is required.");
            if(string.IsNullOrWhiteSpace(dto.name))
                throw new ArgumentException("Post category name cannot be empty.", nameof(dto.name));

            var category = _mapper.Map<PostCategory>(dto);
            var result = await _unitOfWork.PostCategoryRepository.AddPostCategoryAsync(category);
            _logger.LogInformation("Post category added with Id {CategoryId}", result?.Id);
            return _mapper.Map<RetriveCategoryDTO>(result);
        }

        public async Task<RetriveCategoryDTO?> UpdatePostCategoryAsync(int Id, PostCategoryDTO dto)
        {
            _logger.LogInformation("Updating post category with Id {CategoryId}", Id);
            var existingCategory = await _unitOfWork.PostCategoryRepository.GetPostCategoryByIdAsync(Id);
            if (existingCategory is null)
            {
                throw new KeyNotFoundException($"Post category with Id {Id} not exits.");
            }

            var category = _mapper.Map(dto, existingCategory);
            var result =  await _unitOfWork.PostCategoryRepository.UpdatePostCategoryAsync(category);
            _logger.LogInformation("Post category updated with Id {CategoryId}", result?.Id);
            return _mapper.Map<RetriveCategoryDTO>(result);
        }

        public async Task<bool> DeletePostCategoryAsync(int Id)
        {
            _logger.LogInformation("Deleting post category with Id {CategoryId}", Id);
            var existingCategory = await _unitOfWork.PostCategoryRepository.GetPostCategoryByIdAsync(Id);
            if(existingCategory is null)
            {
                throw new KeyNotFoundException($"Post category with Id {Id} not exits.");
            }
            var result = await _unitOfWork.PostCategoryRepository.DeletePostCategoryAsync(Id);
            _logger.LogInformation("Post category with Id {CategoryId} deleted successfully", Id);
            return result;
        }
    }
}
