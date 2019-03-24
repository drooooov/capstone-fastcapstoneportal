using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FASTCapstonePortal.RequestModels
{
    public class ProjectCreate
    {
        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Difficulty { get; set; }

        [Required]
        public int IPType { get; set; }

        [Required]
        public string ClientName { get; set; }

        [JsonProperty("clientEmail")]
        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; }

        [Required]
        public string ClientContact { get; set; }

        public Project NewProject()
        {
            return new Project
            {
                ProjectName = ProjectName,
                Description = Description,
                Difficulty = Difficulty,
                IPType = IPType,
                ClientName = ClientName,
                ClientEmail = ClientEmail,
                ClientContact = ClientContact,
                Approved = true
            };
        }
    }
}
