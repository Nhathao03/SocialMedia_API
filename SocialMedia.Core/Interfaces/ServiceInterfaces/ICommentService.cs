using SocialMedia.Core.DTO.Comment;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO.Comment;

namespace SocialMedia.Core.Services
{
    public interface ICommentService
    {
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<List<Comment>?> GetCommentByPostIdAsync(int id);
        Task<RetriveCommentDTO?> AddCommentAsync(CommentDTO comment);
        Task<RetriveCommentDTO?> UpdateCommentAsync(int id, CommentDTO comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
