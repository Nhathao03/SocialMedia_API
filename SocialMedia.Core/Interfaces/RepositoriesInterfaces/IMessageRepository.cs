using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Repositories
{
    public interface IMessageRepository
    {
        Task<List<Message>?> GetAllMessagesAsync();
        Task<Message?> GetMessageByIdAsync(int Id);
        Task<Message?> AddMessageAsync(Message Message);
        Task<Message?> UpdateMessageAsync(Message Message);
        Task<bool> DeleteMessageAsync(int Id);
        Task<List<Message>?> GetMessageByReceiverIdAndSenderIdAsync(string userId1, string userId2);
        Task<List<Message>?> GetMessageLastestAsync(string userId1, string userId2);
    }
}
