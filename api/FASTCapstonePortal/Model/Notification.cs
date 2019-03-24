using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FASTCapstonePortal.Model
{
    public class Notification
    {

        public int Id { get; set; }

        [Required]
        public bool Read { get; set; }

        [ForeignKey(nameof(Receiver))]
        public int ReceiverId { get; set; }
        [Required]
        public virtual CapstoneUser Receiver { get; set; }

        [ForeignKey(nameof(NotificationContext))]
        public int NotificationContextId { get; set; }
        [Required]
        public virtual NotificationContext NotificationContext { get; set; }
        
    }
}
