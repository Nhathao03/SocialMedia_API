using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(int Id);
        Task<List<Comment>?> GetCommentByPostIdAsync(int postId);
        Task<Comment?> AddCommentAsync(Comment comment);
        Task<Comment?> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int Id);
    }
}
