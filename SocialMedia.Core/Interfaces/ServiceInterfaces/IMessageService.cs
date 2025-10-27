using SocialMedia.Core.DTO.Message;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Services
{
    public interface IMessageService
    {
        Task<List<Message>?> GetAllMessageAsync();
        Task<Message?> GetMessageByIdAsync(int id);
        Task<RetriveMessageDTO?> AddMessageAsync(MessageDTO model);
        Task<RetriveMessageDTO?> UpdateMessageAsync(int id, MessageDTO model);
        Task<bool> DeleteMessageAsync(int id);
        Task<List<Message>?> GetMessageByReceiverIdAndSenderIdAsync(string userId1, string userId2);
        Task<List<Message>?> GetMessageLastestAsync(string userId1, string userId2);
    }
}
