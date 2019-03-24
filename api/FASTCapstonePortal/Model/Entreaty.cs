using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FASTCapstonePortal.Model
{
    public class Entreaty
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }
        
        public bool Accepted { get; set; }

        public virtual NotificationContext NotificationContext { get; set; }

        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        [Required]
        public virtual Student Student { get; set; }
        
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        [Required]
        public virtual Group Group { get; set; }

        public EntreatyType EntreatyType { get; set; }
    }

    public enum EntreatyType
    {
        REQUEST,
        INVITE
    }
}
