using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO.Comment;

namespace SocialMedia.Core.Services
{
    public interface ICommentRepliesService
    {
        Task<IEnumerable<CommentReplies>> GetAllCommentReplies();
        Task<CommentReplies> GetCommentRepliesById(int id);
        Task UpdateCommentReplies(CommentRepliesDTO model);
        Task DeleteCommentReplies(int id);
        Task<int> AddCommentReplies(CommentRepliesDTO model);
    }
}
