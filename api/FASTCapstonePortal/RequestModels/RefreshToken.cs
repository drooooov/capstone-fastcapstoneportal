using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FASTCapstonePortal.RequestModels
{
    public class RefreshToken
    {
        [JsonProperty("access_token")]
        [Required]
        public string Access_Token { get; set; }

        [JsonProperty("refresh_token")]
        [Required]
        public string Refresh_Token { get; set; }
    }
}
