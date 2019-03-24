using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;

namespace FASTCapstonePortal.ResponseModels
{
    public class StudentResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("program")]
        public Programs Program { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

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

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("groupId")]
        public int? GroupId { get; set; }

        [JsonProperty("skills")]
        public IEnumerable<string> Skills { get; set; }

        public StudentResponse(Student s)
        {
            FirstName = s.FirstName;
            LastName = s.LastName;
            Id = s.Id;
            Campus = s.Campus;
            Program = s.Program;
            Picture = s.Picture;
            Description = s.Description;
            PortfolioLink = s.PortfolioLink;
            LinkedInLink = s.LinkedInLink;
            Role = s.Role;
            Email = s.User.Email;
            if (s.Skills == null) Skills = null;
            else Skills = s.Skills.Select(ss => ss.Skill.Name);
            if (s.Group == null) GroupId = null;
            else GroupId = s.Group.Id;
        }
    }
}
