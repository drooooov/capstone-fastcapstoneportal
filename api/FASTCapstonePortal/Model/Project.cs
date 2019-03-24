using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FASTCapstonePortal.Model
{
    public class Project
    {

        public Project()
        {
            PreferredByGroups = new HashSet<GroupsProjects>();
        }

        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Difficulty { get; set; }

        [Required]
        public int IPType { get; set; }

        [Required]
        public bool Approved { get; set; }

        [Required]
        public bool Proposed { get; set; }

        [Required]
        public string ClientName { get; set; }

        [Required]
        public string ClientEmail { get; set; }

        [Required]
        public string ClientContact { get; set; }

        public string Comments { get; set; }

        [InverseProperty("FinalProjectAssigned")]
        public virtual Group AssignedGroup { get; set; }

        public virtual ICollection<GroupsProjects> PreferredByGroups { get; set; }
    }
}
