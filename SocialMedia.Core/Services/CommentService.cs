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

        public async Task<Comment?> GetCommentByIdAsync(int Id)
        {
            return await _unitOfWork.CommentRepository.GetCommentByIdAsync(Id);
        }
        public async Task<List<Comment>?> GetCommentByPostIdAsync(int Id)
        {
            return await _unitOfWork.CommentRepository.GetCommentByPostIdAsync(Id);
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
            _logger.LogInformation("Comment added with Id {CommentId}", result?.Id);
            return _mapper.Map<RetriveCommentDTO>(result);
        }

        public async Task<RetriveCommentDTO?> UpdateCommentAsync(int Id, CommentDTO dto)
        {
            _logger.LogInformation("Updating comment with Id {CommentId}", Id);
            var existingComment = await _unitOfWork.CommentRepository.GetCommentByIdAsync(Id);
            if (existingComment is null)
            {
                throw new KeyNotFoundException($"Comment with Id {Id} not exits.");
            }
            var comment = _mapper.Map(dto, existingComment);
            var result =  await _unitOfWork.CommentRepository.UpdateCommentAsync(comment);
            _logger.LogInformation("Comment updated with Id {CommentId}", result?.Id);
            return _mapper.Map<RetriveCommentDTO>(result);
        }

        public async Task<bool> DeleteCommentAsync(int Id)
        {
             _logger.LogInformation("Deleting comment with Id {CommentId}", Id);
            var existingComment = await _unitOfWork.CommentRepository.GetCommentByIdAsync(Id);
            if (existingComment is null)
            {
                throw new KeyNotFoundException($"Comment with Id {Id} not exits.");
            }
            var result = await _unitOfWork.CommentRepository.DeleteCommentAsync(Id);
            _logger.LogInformation("Comment with Id {CommentId} deleted successfully", Id);
            return result;
        }
    }
}
