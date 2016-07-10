using Xamarin.Forms;

namespace TweetPockets.Views
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public TimelinePage Timeline
        {
            get { return this.TimelinePage; }
        }
    }
}
