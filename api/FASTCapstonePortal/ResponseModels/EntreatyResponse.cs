using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using System;

namespace FASTCapstonePortal.ResponseModels
{
    public class EntreatyResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("accepted")]
        public bool Accepted { get; set; }

        [JsonProperty("student")]
        public int Student { get; set; }

        [JsonProperty("group")]
        public int Group { get; set; }

        [JsonProperty("entreatyType")]
        public EntreatyType EntreatyType { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        public EntreatyResponse(Entreaty entreaty)
        {
            Id = entreaty.Id;
            Message = entreaty.Message;
            Accepted = entreaty.Accepted;
            Student = entreaty.Student.Id;
            Group = entreaty.Group.Id;
            EntreatyType = entreaty.EntreatyType;
            Time = entreaty.NotificationContext.Time;
        }
    }
}
