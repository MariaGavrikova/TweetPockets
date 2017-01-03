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
using TweetPockets.Interfaces.Entities;
using TweetPockets.Utils;
using Xamarin.Forms;

namespace TweetPockets.ViewModels.Entities
{
    public class EventViewModel : ViewModelBase, IEntity
    {
        private readonly StreamContent _content;

        public EventViewModel()
        {
            
        }

        public EventViewModel(StreamContent content)
        {
            _content = content;
            if (content.Entity is Event)
            {
                ParseEvent(content);
            }
            else if (content.Entity is Status)
            {
                var status = (Status) content.Entity;
                CreatedAt = status.CreatedAt;
                StatusId = (long) status.StatusID;
                InitiatorId = long.Parse(status.User.UserIDResponse);
                InitiatorName = status.User.Name;

                if (status.RetweetedStatus != null && status.RetweetedStatus.StatusID != 0)
                {
                    Text = status.RetweetedStatus.Text;
                    TargetUserId = (long) status.RetweetedStatus.UserID;
                    EventType = UserStreamEventType.Retweet;
                }
                else if (status.QuotedStatus != null && status.QuotedStatus.StatusID != 0)
                {
                    Text = status.QuotedStatus.Text;
                    TargetUserId = (long)status.QuotedStatus.UserID;
                    EventType = UserStreamEventType.Quoted;
                }
                else if (status.InReplyToUserID != 0)
                {
                    TargetUserId = (long) status.InReplyToUserID;
                    Text = status.Text;
                    EventType = UserStreamEventType.Replied;
                }
            }
            else if (content.Entity is Delete)
            {
                var delete = (Delete) content.Entity;
                InitiatorId = (long) delete.UserID;
                StatusId = (long) delete.StatusID;
                EventType = UserStreamEventType.Unknown;
            }
        }


        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        [Ignore]
        public string TimestampLabel
        {
            get
            {
                return TimestampHelper.GetText(CreatedAt);
            }
        }

        public UserStreamEventType EventType { get; set; }

        public long InitiatorId { get; set; }

        public string InitiatorName { get; set; }

        public long TargetUserId { get; set; }

        public long StatusId { get; set; }

        public string Text { get; set; }

        private void ParseEvent(StreamContent content)
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
            InitiatorId = long.Parse(initiator.GetValue<string>("id_str"));
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
                    StatusId = long.Parse(targetObject.GetValue<string>("id_str"));
                    Text = targetObject.GetValue<string>("text");
                }
            }
        }
    }

    public enum UserStreamEventType
    {
        Unknown,
        Follow,
        UserUpdate,
        Favorite,
        Unfavorite,
        Retweet,
        Quoted,
        Replied,
        Block,
        Unblock,
        ListCreated,
        ListDestroyed,
        ListUpdated,
        ListMemberAdded,
        ListMemberRemoved,
        ListUserSubscribed,
        ListUserUnsubscribed,
    }

}
