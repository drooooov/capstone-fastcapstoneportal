using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FASTCapstonePortal.Model
{
    public class GroupsProjects
    { 
        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        [Required]
        public virtual Group Group { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        [Required]
        public virtual Project Project { get; set; }

        [Required]
        public int Preference { get; set; }
    }
}
