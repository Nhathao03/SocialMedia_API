using SocialMedia.Core.DTO.Message;
using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Services
{
    public interface IMessageService
    {
        Task<List<Message>?> GetAllMessageAsync();
        Task<Message?> GetMessageByIdAsync(int Id);
        Task<RetriveMessageDTO?> AddMessageAsync(MessageDTO model);
        Task<RetriveMessageDTO?> UpdateMessageAsync(int Id, MessageDTO model);
        Task<bool> DeleteMessageAsync(int Id);
        Task<List<Message>?> GetMessageByReceiverIdAndSenderIdAsync(string userId1, string userId2);
        Task<List<Message>?> GetMessageLastestAsync(string userId1, string userId2);
    }
}
