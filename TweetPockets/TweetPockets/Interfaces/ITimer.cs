using System;

namespace TweetPockets.Interfaces
{
    public interface ITimer
    {
        event EventHandler Tick;

        TimeSpan Interval { get; set; }

        void Start();

        void Stop();
    }
}