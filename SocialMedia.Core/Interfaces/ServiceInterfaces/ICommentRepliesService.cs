using SocialMedia.Core.DTO.Comment;
using SocialMedia.Core.Entities.CommentEntity;
using SocialMedia.Core.Entities.DTO.Comment;

namespace SocialMedia.Core.Services
{
    public interface ICommentRepliesService
    {
        Task<CommentReplies?> GetCommentRepliesByIdAsync(int id);
        Task<RetriveCommentRepliesDTO?> UpdateCommentRepliesAsync(int id, CommentRepliesDTO model);
        Task<bool> DeleteCommentRepliesAsync(int id);
        Task<RetriveCommentRepliesDTO?> AddCommentRepliesAsync(CommentRepliesDTO model);
    }
}
