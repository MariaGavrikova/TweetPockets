using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetPockets.ViewModels;
using TweetPockets.Views;
using Xamarin.Forms;

namespace TweetPockets.Managers
{
    public class ViewManager
    {
        private Dictionary<Type, Page> _mapping = new Dictionary<Type, Page>(); 

        public void Register(ViewModelBase viewModel, Page view)
        {
            _mapping.Add(viewModel.GetType(), view);
        }

        public Page GetView(ViewModelBase viewModel)
        {
            var view = _mapping[viewModel.GetType()];
            view.BindingContext = viewModel;
            return view;
        }
    }
}
