using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FASTCapstonePortal.RequestModels
{
    public class LoginUser
    {
        [JsonProperty("password")]
        [Required]
        public string Password { get; set; }

        [JsonProperty("username")]
        [Required]
        public string UserName { get; set; }
    }
}
