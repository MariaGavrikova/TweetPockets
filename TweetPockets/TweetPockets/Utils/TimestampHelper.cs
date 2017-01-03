using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.Resources;

namespace TweetPockets.Utils
{
    public class TimestampHelper
    {
        public static string GetText(DateTime createdAt)
        {
            var timespan = DateTime.UtcNow - createdAt;
            string result;
            if (timespan.Days > 0)
            {
                result = String.Format(AppResources.DaysLabel, timespan.Days);
            }
            else
            {
                if (timespan.Hours > 0)
                {
                    result = String.Format(AppResources.HoursLabel, timespan.Hours);
                }
                else
                {
                    if (timespan.Minutes > 0)
                    {
                        result = String.Format(AppResources.MinutesLabel, timespan.Minutes);
                    }
                    else
                    {
                        result = String.Format(AppResources.SecondsLabel, Math.Max(timespan.Seconds, 1));
                    }
                }
            }

            return result;
        }
    }
}
