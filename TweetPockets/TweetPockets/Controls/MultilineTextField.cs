using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace TweetPockets.Controls
{
    public class MultilineTextField : View
    {
        public event EventHandler Valid;
        public event EventHandler Invalid;

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<MultilineTextField, string>(p => p.Text, String.Empty, BindingMode.TwoWay);
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly BindableProperty LabelProperty =
            BindableProperty.Create<MultilineTextField, string>(p => p.Label, String.Empty);
        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }
            set
            {
                SetValue(LabelProperty, value);
            }
        }

        public void RaiseValid()
        {
            Valid?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseInvalid()
        {
            Invalid?.Invoke(this, EventArgs.Empty);
        }
    }
}
