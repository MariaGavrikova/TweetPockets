using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using LinqToTwitter.Common;
using LitJson;
using SQLite.Net.Attributes;

namespace TweetPockets.ViewModels.Entities
{
    public class EventViewModel : ViewModelBase
    {
        public EventViewModel()
        {
            
        }

        public EventViewModel(StreamContent content)
        {
            var parsed = JsonMapper.ToObject(content.Content);

            var eventAsString = parsed.GetValue<string>("event");

            try
            {
                EventType = (UserStreamEventType)Enum.Parse(
                    typeof(UserStreamEventType),
                    eventAsString.Replace("_", ""),
                    true);
            }
            catch
            {
                EventType = UserStreamEventType.Unknown;
            }

            CreatedAt = DateTime.ParseExact(parsed.GetValue<string>("created_at"),
                "ddd MMM dd HH:mm:ss %zzzz yyyy", CultureInfo.InvariantCulture);

            var initiator = parsed["source"];
            InitiatorId = ulong.Parse(initiator.GetValue<string>("id_str"));
            InitiatorName = initiator.GetValue<string>("name");
            //Target = new User(parsed["target"]);

            var targetObject = parsed.GetValue<JsonData>("target_object");
            if (targetObject != null)
            {
                if (targetObject.GetValue<JsonData>("mode") != null)
                {
                    //TargetList = new List(targetObject);
                }
                else if (targetObject.GetValue<JsonData>("text") != null
                    && targetObject.GetValue<JsonData>("sender") == null)
                {
                    //TargetStatus = new Status(targetObject);
                    Text = targetObject.GetValue<string>("text");
                }
            }
        }

        public DateTime CreatedAt { get; set; }

        [Ignore]
        public DateTime CreatedAtLocal => CreatedAt.ToLocalTime();

        public UserStreamEventType EventType { get; set; }

        public ulong InitiatorId { get; set; }

        public string InitiatorName { get; set; }

        public string Text { get; set; }
    }

    public enum UserStreamEventType
    {
        Unknown,
        Follow,
        UserUpdate,
        Favorite,
        Unfavorite,
        Block,
        Unblock,
        ListCreated,
        ListDestroyed,
        ListUpdated,
        ListMemberAdded,
        ListMemberRemoved,
        ListUserSubscribed,
        ListUserUnsubscribed
    }

}
