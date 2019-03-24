using FASTCapstonePortal.Model;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FASTCapstonePortal
{
    public class CapstoneUser : IdentityUser<int>
    {
        public CapstoneUser() : base()
        {
            CreatedNotifications = new HashSet<NotificationContext>();
            ReceivedNotifications = new HashSet<Notification>();
        }

        [Required]
        public bool Admin { get; set; }
        
        public string RefreshToken { get; set; }

        public virtual ICollection<NotificationContext> CreatedNotifications { get; set; }

        public virtual ICollection<Notification> ReceivedNotifications { get; set; }
    }
}
