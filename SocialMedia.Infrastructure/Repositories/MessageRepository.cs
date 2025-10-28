using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>?> GetAllMessagesAsync()
        {
            return await _context.messages.ToListAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(int id)
        {
            return await _context.messages.FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task<Message?> AddMessageAsync(Message Message)
        {          
            _context.AddAsync(Message);
            await _context.SaveChangesAsync();
            return Message;
        }

        public async Task<Message?> UpdateMessageAsync(Message Message)
        {
            _context.messages.Update(Message);
            await _context.SaveChangesAsync();
            return Message;
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            var Message = await _context.messages.FindAsync(id);
            if(Message is null)
                return false;
            _context.messages.Remove(Message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Message>?> GetMessageByReceiverIdAndSenderIdAsync(string userId1, string userId2)
        {
            return await _context.messages.Where(m => (m.ReceiverId == userId1 && m.SenderId == userId2) 
                                                || (m.ReceiverId == userId2 && m.SenderId == userId1))
                                                .OrderBy(m => m.CreatedAt).ToListAsync();
        }

        public async Task<List<Message>?> GetMessageLastestAsync( string userId1, string userId2)
        {
            return await _context.messages.Where(m => (m.ReceiverId == userId1 && m.SenderId == userId2)
                                                           || (m.ReceiverId == userId2 && m.SenderId == userId1))
                                                .OrderByDescending(m => m.CreatedAt).Take(1).ToListAsync();
        }

    }
}
