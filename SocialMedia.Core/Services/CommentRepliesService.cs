using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Core.Entities.DTO.Comment;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Interfaces.ServiceInterfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.DTO.Comment;

namespace SocialMedia.Core.Services 
{
    public class CommentRepliesService : ICommentRepliesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentRepliesService> _logger;
        public CommentRepliesService(IUnitOfWork unitOfWork,
            ILogger<CommentRepliesService> logger,
            IMapper mapper)
        {
              _mapper = mapper;
              _unitOfWork = unitOfWork;
              _logger = logger;
        }

        public async Task<CommentReplies?> GetCommentRepliesByIdAsync(int id)
        {
            return await _unitOfWork.CommentRepliesRepository.GetCommentRepliesByIdAsync(id);
        }

        public async Task<RetriveCommentRepliesDTO?> UpdateCommentRepliesAsync(int id, CommentRepliesDTO dto)
        {
            _logger.LogInformation("Updating comment reply with ID {CommentReplyID}", id);
            var existingAddress = await _unitOfWork.CommentRepliesRepository.GetCommentRepliesByIdAsync(id);
            if (existingAddress == null)
            {
                throw new KeyNotFoundException($"Address với Id {id} không tồn tại.");
            }
            var crp = _mapper.Map(dto, existingAddress);

            var result = await _unitOfWork.CommentRepliesRepository.UpdateCommentRepliesAsync(existingAddress);
            _logger.LogInformation("Comment reply updated with ID {CommentReplyID}", result?.Id);
            return _mapper.Map<RetriveCommentRepliesDTO>(result);
        }

        public async Task<bool> DeleteCommentRepliesAsync(int id)
        {
            _logger.LogInformation("Deleting comment reply with ID {CommentReplyID}", id);
            var existingCommentReply = await _unitOfWork.CommentRepliesRepository.GetCommentRepliesByIdAsync(id);
            if (existingCommentReply == null)
            {
                throw new KeyNotFoundException($"Comment reply with Id {id} not found.");
            }
            var result = await _unitOfWork.CommentRepliesRepository.DeleteCommentRepliesAsync(id);
            _logger.LogInformation("Comment reply with ID {CommentReplyID} deleted successfully.", id);
            return result;
        }

        public async Task<RetriveCommentRepliesDTO?> AddCommentRepliesAsync(CommentRepliesDTO model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Comment reply data is required.");

            if (string.IsNullOrWhiteSpace(model.Content))
                throw new ArgumentException("Reply content cannot be empty.", nameof(model.Content));

            var crp = _mapper.Map<CommentReplies>(model);
            var result = await _unitOfWork.CommentRepliesRepository.AddNewCommentRepliesAsync(crp);
            _logger.LogInformation("New comment reply added with ID {CommentReplyID}", result?.Id);
            return _mapper.Map<RetriveCommentRepliesDTO>(result);
        }

    }
}
