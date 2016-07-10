using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace TweetPockets
{
    public class PhotoStatusViewModel : StatusViewModel
    {
        public PhotoStatusViewModel(Status model) : base(model)
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
