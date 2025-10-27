using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities.DTO.Comment;
using SocialMedia.Core.Entities.CommentEntity;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using SocialMedia.Core.DTO.Comment;
using AutoMapper;

namespace SocialMedia.Core.Services
{
    public class CommentService :  ICommentService
    {
        private readonly ILogger<CommentService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork,
            ILogger<CommentService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _unitOfWork.CommentRepository.GetCommentByIdAsync(id);
        }
        public async Task<List<Comment>?> GetCommentByPostIdAsync(int id)
        {
            return await _unitOfWork.CommentRepository.GetCommentByPostIdAsync(id);
        }

        public async Task<RetriveCommentDTO?> AddCommentAsync(CommentDTO dto)
        {
            _logger.LogInformation("Adding a new comment");
            if (dto is null)
                throw new ArgumentNullException(nameof(CommentDTO), "Comment data is required.");
            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Comment content cannot be empty.", nameof(dto.Content));

            var comment = _mapper.Map<Comment>(dto);
            var result = await _unitOfWork.CommentRepository.AddCommentAsync(comment);
            _logger.LogInformation("Comment added with ID {CommentId}", result?.ID);
            return _mapper.Map<RetriveCommentDTO>(result);
        }

        public async Task<RetriveCommentDTO?> UpdateCommentAsync(int id, CommentDTO dto)
        {
            _logger.LogInformation("Updating comment with ID {CommentId}", id);
            var existingComment = await _unitOfWork.CommentRepository.GetCommentByIdAsync(id);
            if (existingComment is null)
            {
                throw new KeyNotFoundException($"Comment with Id {id} not exits.");
            }
            var comment = _mapper.Map(dto, existingComment);
            var result =  await _unitOfWork.CommentRepository.UpdateCommentAsync(comment);
            _logger.LogInformation("Comment updated with ID {CommentId}", result?.ID);
            return _mapper.Map<RetriveCommentDTO>(result);
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
             _logger.LogInformation("Deleting comment with ID {CommentId}", id);
            var existingComment = await _unitOfWork.CommentRepository.GetCommentByIdAsync(id);
            if (existingComment is null)
            {
                throw new KeyNotFoundException($"Comment with Id {id} not exits.");
            }
            var result = await _unitOfWork.CommentRepository.DeleteCommentAsync(id);
            _logger.LogInformation("Comment with ID {CommentId} deleted successfully", id);
            return result;
        }
    }
}
