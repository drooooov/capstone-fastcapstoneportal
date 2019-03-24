using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.ResponseModels
{
    public class ProjectResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("difficulty")]
        public int Difficulty { get; set; }

        [JsonProperty("ipType")]
        public int IPType { get; set; }

        [JsonProperty("approved")]
        public bool Approved { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("clientEmail")]
        public string ClientEmail { get; set; }

        [JsonProperty("clientContract")]
        public string ClientContact { get; set; }

        public string Comments { get; set; }

        public Dictionary<int,int> PreferredByGroupIds { get; set; }

        public ProjectResponse(Project p)
        {
            Id = p.Id;
            ProjectName = p.ProjectName;
            Description = p.Description;
            Difficulty = p.Difficulty;
            IPType = p.IPType;
            Approved = p.Approved;
            ClientName = p.ClientName;
            ClientEmail = p.ClientEmail;
            ClientContact = p.ClientContact;
            Comments = p.Comments;
            if (p.PreferredByGroups == null) PreferredByGroupIds = null;
            else PreferredByGroupIds = p.PreferredByGroups.ToDictionary(gp => gp.GroupId, gp => gp.Preference);

        }
    }
}
