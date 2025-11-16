using SocialMedia.Core.DTO.Comment;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO.Comment;

namespace SocialMedia.Core.Services
{
    public interface ICommentService
    {
        Task<Comment?> GetCommentByIdAsync(int Id);
        Task<List<Comment>?> GetCommentByPostIdAsync(int Id);
        Task<RetriveCommentDTO?> AddCommentAsync(CommentDTO comment);
        Task<RetriveCommentDTO?> UpdateCommentAsync(int Id, CommentDTO comment);
        Task<bool> DeleteCommentAsync(int Id);
    }
}
