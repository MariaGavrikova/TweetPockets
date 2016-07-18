using System.Collections.Generic;
using LinqToTwitter;

namespace TweetPockets.ViewModels
{
    public class PhotoStatusViewModel : StatusViewModel
    {
        public PhotoStatusViewModel(Status model, int i) : base(model, i)
        {
            var photos = new List<string>();
            foreach (var mediaEntity in model.Entities.MediaEntities)
            {
                photos.Add(mediaEntity.MediaUrl);
            }
            PhotoUrls = photos;
        }

        public IEnumerable<string> PhotoUrls { get; private set; }
    }
}
