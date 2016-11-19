using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TweetPockets.ViewModels;
using TweetPockets.Views;
using Xamarin.Forms;

namespace TweetPockets.Managers
{
    public class ViewManager
    {
        private Dictionary<Type, Page> _mapping = new Dictionary<Type, Page>(); 

        public void Register<T, TPage>()
            where T : ViewModelBase
            where TPage : ContentPage, new()
        {
            _mapping.Add(typeof(T), new TPage());
        }

        public Page GetView(ViewModelBase viewModel)
        {
            var view = _mapping[viewModel.GetType()];
            view.BindingContext = viewModel;
            return view;
        }
    }
}
