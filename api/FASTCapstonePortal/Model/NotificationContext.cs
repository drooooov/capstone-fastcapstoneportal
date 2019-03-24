using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FASTCapstonePortal.Model
{
    public class NotificationContext
    {

        public NotificationContext()
        {
            NotificationsSent = new HashSet<Notification>();
        }

        public int Id { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public virtual CapstoneUser CreatedBy { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        public virtual ICollection<Notification> NotificationsSent { get; set; }
    }

    public enum NotificationType
    {
        ENTREATY,
        ANNOUNCEMENT,
        SYSTEM
    }
}
