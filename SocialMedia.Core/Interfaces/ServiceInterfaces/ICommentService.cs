using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO.Comment;

namespace SocialMedia.Core.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(int id);
        Task<IEnumerable<Comment>> GetCommentByPostIDAsync(int postID);
        Task<int> AddCommentAsync(CommentDTO comment);
        Task UpdateCommentAsync(CommentDTO comment);
        Task DeleteCommentAsync(int id);
        Task LikeCommentAsync(int commentId, string userId);
    }
}
