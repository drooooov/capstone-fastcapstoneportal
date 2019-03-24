using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.RequestModels
{
    public class ProjectUpdate
    {
        [Required]
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [Required]
        [JsonProperty("description")]
        public string Description { get; set; }

        [Required]
        [JsonProperty("difficulty")]
        public int Difficulty { get; set; }

        [Required]
        [JsonProperty("ipType")]
        public int IPType { get; set; }

        [Required]
        [JsonProperty("approved")]
        public bool Approved { get; set; }

        [Required]
        [JsonProperty("proposed")]
        public bool Proposed { get; set; }

        [Required]
        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [Required]
        [JsonProperty("clientEmail")]
        public string ClientEmail { get; set; }

        [Required]
        [JsonProperty("clientContact")]
        public string ClientContact { get; set; }

        [Required]
        [JsonProperty("comments")]
        public string Comments { get; set; }
    }
}
