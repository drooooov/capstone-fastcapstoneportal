using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace FASTCapstonePortal.RequestModels
{
    public class RegisterUser
    {
        [JsonProperty("admin")]
        [Required]
        public bool Admin { get; set; }

        [JsonProperty("program")]
        [Required]
        public Programs Program { get; set; }

        [JsonProperty("password")]
        [Required]
        public string Password { get; set; }

        [JsonProperty("firstName")]
        [Required]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        [Required]
        public string LastName { get; set; }

        [JsonProperty("userName")]
        [Required]
        public string UserName { get; set; }

        [JsonProperty("email")]
        [Required]
        [EmailAddress]
        public string Email {get; set;}
    }
}
