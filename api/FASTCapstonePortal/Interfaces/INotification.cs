using FASTCapstonePortal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Interfaces
{
    public interface INotification
    {
        Task<IEnumerable<Notification>> GetAllNotificationsWithReceiverIdAsync(int receiverId);
        Task<IEnumerable<Notification>> GetAllWithTimeAsync(DateTime moment);
        Task<IEnumerable<Notification>> GetAllReadNotificationsAsync(int userId, int pastWeeks);
        Task<IEnumerable<Notification>> GetAllUnreadNotificationsAsync(int userId, int pastWeeks);
        Task<IEnumerable<NotificationContext>> GetAllSentNotificationsInPastWeekAsync(int senderId, int pastWeeks);
        Task<IEnumerable<Notification>> GetAllReceivedNotificationsInPastWeekAsync(int receiverId, int pastWeeks);
        Task SaveAsync();
        Task CreateAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(Notification notification);
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification> GetByIdAsync(int notificationId);
        Task AnnounceToAllStudentsAsync(string message, int userId);
        Task<bool> ExistsAsync(int notificationId);
        Task ReadNotificationAync(Notification notification);
        Task DeleteAnnouncementAsync(NotificationContext notificationContext);
        Task<NotificationContext> GetAnnouncementByIdAsync(int id);
        Task<IEnumerable<NotificationContext>> GetAllAnnouncementsAsync();
    }
}
