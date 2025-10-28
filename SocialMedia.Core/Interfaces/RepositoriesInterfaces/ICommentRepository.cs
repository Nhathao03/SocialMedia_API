using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(int id);
        Task<List<Comment>?> GetCommentByPostIdAsync(int postID);
        Task<Comment?> AddCommentAsync(Comment comment);
        Task<Comment?> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
