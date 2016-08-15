using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TweetPockets.Controls
{
    public class CircleImage : View
    {
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create<CircleImage, string>(p => p.Source, null);

        public string Source
        {
            get
            {
                return (string)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }
    }
}
