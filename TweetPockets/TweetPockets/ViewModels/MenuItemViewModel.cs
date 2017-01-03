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
        private readonly string _selectedImage;
        private readonly string _defaultImage;
        private string _image;
        private bool _isSelected;

        protected MenuItemViewModel(string title, string defaultImage, string selectedImage)
        {
            _defaultImage = defaultImage;
            _selectedImage = selectedImage;
            Title = title;
            Image = defaultImage;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                Image = _isSelected ? _selectedImage : _defaultImage;
                OnPropertyChanged();
            }
        }

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value; 
                OnPropertyChanged();
            }
        }

        public string Title { get; set; }

        public virtual async Task Reload()
        {
        }
    }
}
