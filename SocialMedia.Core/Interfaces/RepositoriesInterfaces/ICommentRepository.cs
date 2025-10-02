using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllComments();
        Task<Comment> GetCommentById(int id);
        Task<IEnumerable<Comment>> GetCommentByPostID(int postID);
        Task<int> AddComment(Comment comment);
        Task UpdateComment(Comment comment);
        Task DeleteComment(int id);
    }
}
