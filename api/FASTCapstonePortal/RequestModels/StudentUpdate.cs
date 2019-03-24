using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FASTCapstonePortal.RequestModels
{
    public class StudentUpdate
    {
        [Required]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [Required]
        [JsonProperty("campus")]
        public Campuses Campus { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("linkedinLink")]
        public string LinkedInLink { get; set; }

        [JsonProperty("portfolioLink")]
        public string PortfolioLink { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("skills")]
        public HashSet<string> Skills { get; set; }
    }
}
