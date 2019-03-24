using FASTCapstonePortal.Model;
using Newtonsoft.Json;
using System;

namespace FASTCapstonePortal.ResponseModels
{
    public class AnnouncementResponse
        {
            [JsonProperty("id")]
            public int Id;

            [JsonProperty("data")]
            public string Data { get; set; }

            [JsonProperty("time")]
            public DateTime Time { get; set; }

            public AnnouncementResponse(NotificationContext notificationContext)
            {
                Id = notificationContext.Id;
                Data = notificationContext.Data;
                Time = notificationContext.Time;
            }
        }
    }
