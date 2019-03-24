using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.ResponseModels
{
    public class GroupResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("scmLink")]
        public string SCMLink { get; set; }
        

        [JsonProperty("proposedProjectId")]
        public int? ProposedProjectId { get; set; }

        [JsonProperty("assignedProjectId")]
        public int? AssignedProjectId { get; set; }

        [JsonProperty("preferredProjectIds")]
        public IEnumerable<int> PreferredProjectIds { get; set; }

        [JsonProperty("memberIds")]
        public IEnumerable<int> MemberIds { get; set; }

        [JsonProperty("groupAdmin")]
        public int GroupAdmin { get; set; }

        public GroupResponse(Group g)
        {
            Id = g.Id;
            Name = g.Name;
            Picture = g.Picture;
            Description = g.Description;
            SCMLink = g.SCMLink;
            GroupAdmin = g.Students.Where(s => s.GroupAdmin == true).Select(s => s.Id).FirstOrDefault();
            if (g.ProposedProject == null) ProposedProjectId = null;
            else ProposedProjectId = g.ProposedProject.Id;
            if (g.FinalProjectAssigned == null) AssignedProjectId = null;
            else AssignedProjectId = g.FinalProjectAssigned.Id;
            if (g.PreferredProjects == null) PreferredProjectIds = null;
            else PreferredProjectIds = g.PreferredProjects.Select(gp => gp.ProjectId);
            MemberIds = g.Students.Select(s => s.Id);
        }
     }
}
