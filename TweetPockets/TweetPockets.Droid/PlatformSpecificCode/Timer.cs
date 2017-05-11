using System;
using TweetPockets.Droid.PlatformSpecificCode;
using TweetPockets.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(Timer))]

namespace TweetPockets.Droid.PlatformSpecificCode
{
    public class Timer : ITimer
    {
        private System.Timers.Timer _platformTimer;

        public Timer()
        {
            _platformTimer = new System.Timers.Timer();
            _platformTimer.AutoReset = true;
            _platformTimer.Elapsed += (sender, args) => OnTick();
        }

        public event EventHandler Tick;

        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_platformTimer.Interval); }
            set { _platformTimer.Interval = value.TotalMilliseconds; }
        }

        protected virtual void OnTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }

        public void Start()
        {
            _platformTimer.Start();
        }

        public void Stop()
        {
            _platformTimer.Stop();
        }
    }
}