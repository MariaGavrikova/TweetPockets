using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TweetPockets.ViewModels
{
    public class MenuItemViewModel : ViewModelBase
    {
        protected MenuItemViewModel(string title, string image)
        {
            Title = title;
            Image = image;
        }

        public string Image { get; set; }

        public string Title { get; set; }
    }
}
