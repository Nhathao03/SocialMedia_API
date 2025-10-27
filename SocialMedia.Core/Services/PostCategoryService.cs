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

        public async Task<PostCategory> GetPostCategoryByIdAsync(int id)
        {
            return await _unitOfWork.PostCategoryRepository.GetPostCategoryByIdAsync(id);
        }

        public async Task<RetriveCategoryDTO?> AddPostCategoryAsync(PostCategoryDTO dto)
        {
            _logger.LogInformation("Adding a new post category with {CategoryName}", dto?.Name);
            if(dto is null)
                throw new ArgumentNullException(nameof(PostCategoryDTO), "Post category data is required.");
            if(string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Post category name cannot be empty.", nameof(dto.Name));

            var category = _mapper.Map<PostCategory>(dto);
            var result = await _unitOfWork.PostCategoryRepository.AddPostCategoryAsync(category);
            _logger.LogInformation("Post category added with ID {CategoryId}", result?.ID);
            return _mapper.Map<RetriveCategoryDTO>(result);
        }

        public async Task<RetriveCategoryDTO?> UpdatePostCategoryAsync(int id, PostCategoryDTO dto)
        {
            _logger.LogInformation("Updating post category with ID {CategoryId}", id);
            var existingCategory = await _unitOfWork.PostCategoryRepository.GetPostCategoryByIdAsync(id);
            if (existingCategory is null)
            {
                throw new KeyNotFoundException($"Post category with Id {id} not exits.");
            }

            var category = _mapper.Map(dto, existingCategory);
            var result =  await _unitOfWork.PostCategoryRepository.UpdatePostCategoryAsync(category);
            _logger.LogInformation("Post category updated with ID {CategoryId}", result?.ID);
            return _mapper.Map<RetriveCategoryDTO>(result);
        }

        public async Task<bool> DeletePostCategoryAsync(int id)
        {
            _logger.LogInformation("Deleting post category with ID {CategoryId}", id);
            var existingCategory = await _unitOfWork.PostCategoryRepository.GetPostCategoryByIdAsync(id);
            if(existingCategory is null)
            {
                throw new KeyNotFoundException($"Post category with Id {id} not exits.");
            }
            var result = await _unitOfWork.PostCategoryRepository.DeletePostCategoryAsync(id);
            _logger.LogInformation("Post category with ID {CategoryId} deleted successfully", id);
            return result;
        }
    }
}
