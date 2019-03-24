using System.ComponentModel.DataAnnotations;

namespace FASTCapstonePortal.Model
{
    public class StudentsSkills
    {
        public int StudentId { get; set; }
        [Required]
        public virtual Student Student { get; set; }
        
        public int SkillId { get; set; }
        [Required]
        public virtual Skill Skill { get; set; }

    }
}
