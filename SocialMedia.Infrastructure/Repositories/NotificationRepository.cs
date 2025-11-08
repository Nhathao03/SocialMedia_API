using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Social_Media.Helpers;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities.DTO;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>?> GetByUserIdAsync(string userId)
        {
            return await _context.notifications.Where(n => n.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Notification>?> GetUnreadByUserIdAsync(string userId)
        {
            return await _context.notifications.Where(n => n.UserId == userId && n.IsRead == false).ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.notifications.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Notification?> AddNotificationAsync(Notification notification)
        {
            _context.notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
        
        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _context.notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var list = await _context.notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();
            foreach (var item in list)
            {
                item.IsRead = true;
            }
            await _context.SaveChangesAsync();
        }
    }
}
