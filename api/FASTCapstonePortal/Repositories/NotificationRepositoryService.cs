using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Repositories
{
    public class NotificationRepositoryService : INotification
    {
        protected readonly CapstoneDBContext _context;
        protected readonly IHubContext<SignalServer> _hubContext;

        public NotificationRepositoryService(CapstoneDBContext context, IHubContext<SignalServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task CreateAsync(Notification notification)
        {
            await _context.AddAsync(notification);
            await SaveAsync();
        }

        public async Task DeleteAsync(Notification notification)
        {
            _context.Remove(notification);
            await SaveAsync();
        }

        public async Task UpdateAsync(Notification entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Set<Notification>().ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Notifications.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsWithReceiverIdAsync(int id)
        {
            return await _context.Notifications.Where(e => e.Receiver.Id == id).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllWithTimeAsync(DateTime time)
        {
            return await _context.Notifications.Where(e => e.NotificationContext.Time == time).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllReceivedNotificationsInPastWeekAsync(int id, int pastWeeks)
        {
            double pastDays = pastWeeks * -7.0;
            return await _context.Notifications
                .Where(e => 
                    DateTime.Compare(e.NotificationContext.Time, DateTime.UtcNow.AddDays(pastDays)) >= 0
                    && 
                    e.Receiver.Id == id
                    )
                .ToListAsync();
        }

        public async Task<IEnumerable<NotificationContext>> GetAllSentNotificationsInPastWeekAsync(int id, int pastWeeks)
        {
            double pastDays = pastWeeks * -7.0;
            return await _context.NotificationContexts
                .Where(e =>
                    DateTime.Compare(e.Time, DateTime.UtcNow.AddDays(pastDays)) >= 0
                    &&
                    e.CreatedBy.Id == id
                    )
                .ToListAsync();
        }

        public async Task AnnounceToAllStudentsAsync(string message, int userId)
        {
            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.Where(u => u.Admin == true).FirstAsync(),
                Data = message,
                NotificationType = NotificationType.ANNOUNCEMENT,
                Time = DateTime.UtcNow
            };
            foreach (Student s in _context.Students)
            {
                notificationContext.NotificationsSent.Add(new Notification()
                {
                    Read = false,
                    Receiver = await _context.Users.FindAsync(s.Id)
                });
            }

            _context.Add(notificationContext);
            await SaveAsync();

            foreach(Notification n in notificationContext.NotificationsSent)
            {
                if (_hubContext.Clients.Group(n.ReceiverId.ToString()) != null)
                    await _hubContext.Clients.Group(n.ReceiverId.ToString()).SendAsync("ReceiveNotification");
            }
  
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetAllUnreadNotificationsAsync(int userId, int pastWeeks)
        {
           return (await GetAllReceivedNotificationsInPastWeekAsync(userId, pastWeeks)).Where(n => n.Read == false);
        }

        public async Task<IEnumerable<Notification>> GetAllReadNotificationsAsync(int userId, int pastWeeks)
        {
            return (await GetAllReceivedNotificationsInPastWeekAsync(userId, pastWeeks)).Where(n => n.Read == true);
        }
        public async Task ReadNotificationAync(Notification notification)
        {
            notification.Read = true;
            await UpdateAsync(notification);
        }

        public async Task DeleteAnnouncementAsync(NotificationContext notificationContext)
        {
            _context.Remove(notificationContext);
            await SaveAsync();
        }

        public async Task<NotificationContext> GetAnnouncementByIdAsync(int id)
        {
            return await _context.NotificationContexts.Where(n => n.Id == id && n.NotificationType == NotificationType.ANNOUNCEMENT).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<NotificationContext>> GetAllAnnouncementsAsync()
        {
            return await _context.NotificationContexts.Where(n => n.NotificationType == NotificationType.ANNOUNCEMENT).ToListAsync();
        }
    }
}
