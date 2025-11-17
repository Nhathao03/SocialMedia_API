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

        public async Task<IEnumerable<Notification>?> GetByUserIdAsync(string senderId)
        {
            return await _context.Notifications.Where(n => n.SenderId == senderId).ToListAsync();
        }

        public async Task<IEnumerable<Notification>?> GetUnreadByUserIdAsync(string SenderId)
        {
            return await _context.Notifications.Where(n => n.SenderId == SenderId && n.IsRead == false).ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int Id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == Id);
        }

        public async Task<Notification?> AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
        
        public async Task MarkAsReadAsync(int Id)
        {
            var notification = await _context.Notifications.FindAsync(Id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(string SenderId)
        {
            var list = await _context.Notifications
				.Where(n => n.SenderId == SenderId && !n.IsRead)
                .ToListAsync();
            foreach (var item in list)
            {
                item.IsRead = true;
            }
            await _context.SaveChangesAsync();
        }
    }
}
