using SocialMedia.Core.Entities.CommentEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface ICommentRepliesRepository
    {
        Task<CommentReplies?> GetCommentRepliesByIdAsync(int id);
        Task<CommentReplies?> UpdateCommentRepliesAsync(CommentReplies commentReplies);
        Task<bool> DeleteCommentRepliesAsync(int id);
        Task<CommentReplies?> AddNewCommentRepliesAsync(CommentReplies commentReplies);
    }
}
