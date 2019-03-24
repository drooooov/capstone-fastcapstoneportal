using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using System;

namespace FASTCapstonePortal.ResponseModels
{
    public class NotificationResponse
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("createdById")]
        public int CreatedBy;

        [JsonProperty("receiverId")]
        public int Receiver;

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("read")]
        public bool Read { get; set; }

        [JsonProperty("notificationType")]
        public NotificationType NotificationType { get; set; }

        public NotificationResponse(Notification notification)
        {
            Id = notification.Id;
            CreatedBy = notification.NotificationContext.CreatedBy.Id;
            Receiver = notification.Receiver.Id;
            Data = notification.NotificationContext.Data;
            Time = notification.NotificationContext.Time;
            Read = notification.Read;
            NotificationType = notification.NotificationContext.NotificationType;
        }
    }
}
