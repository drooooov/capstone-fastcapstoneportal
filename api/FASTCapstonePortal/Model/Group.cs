using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FASTCapstonePortal.Model
{
    public class Group
    {

        public Group()
        {
            PreferredProjects = new HashSet<GroupsProjects>();
            Students = new HashSet<Student>();
            Entreaties = new HashSet<Entreaty>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public string SCMLink { get; set; }

        public virtual Project ProposedProject { get; set; }

        [InverseProperty("AssignedGroup")]
        public virtual Project FinalProjectAssigned { get; set; }

        public virtual ICollection<GroupsProjects> PreferredProjects { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Entreaty> Entreaties { get; set; }
    }
}
