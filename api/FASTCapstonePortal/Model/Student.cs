using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FASTCapstonePortal.Model
{
    public class Student
    {
        public Student()
        {
            Skills = new HashSet<StudentsSkills>();
            Entreaties = new HashSet<Entreaty>();
        }

        [Key, ForeignKey(nameof(User))]
        public int Id { get; set; }
        [Required]
        public virtual CapstoneUser User { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool GroupAdmin { get; set; }

        [Required]
        public Programs Program { get; set; }
       
        public string Role { get; set; }

        public string Picture { get; set; }

        public Campuses Campus { get; set; }

        public string Description { get; set; }

        public string LinkedInLink { get; set; }

        public string PortfolioLink { get; set; }
        
        public virtual Group Group { get; set; }

        public virtual ICollection<StudentsSkills> Skills { get; set; }

        public virtual ICollection<Entreaty> Entreaties { get; set; }
    }

    public enum Programs
    {
        SDNE,
        SA
    }

    public enum Campuses
    {
        DAVIS,
        TRAFALGAR
    }
}
