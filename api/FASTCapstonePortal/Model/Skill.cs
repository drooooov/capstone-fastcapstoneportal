using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FASTCapstonePortal.Model
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<StudentsSkills> Students { get; set; }
    }
}
