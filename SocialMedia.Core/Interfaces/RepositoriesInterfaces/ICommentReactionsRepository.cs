using SocialMedia.Core.Entities.CommentEntity;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface ICommentReactionsRepository
    {
        Task<IEnumerable<CommentReactions>> GetAllCommentReactions();
        Task<CommentReactions> GetCommentReactionsById(int id);
        Task UpdateCommentReactions(CommentReactions commentReactions);
        Task AddNewCommentReactions(CommentReactions commentReactions);
        Task DeleteCommentReactionsById(int id);
    }
}
