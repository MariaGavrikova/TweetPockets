using System.Linq;
using LinqToTwitter;

namespace TweetPockets.ViewModels
{
    class StatusViewModelFactory
    {
        public StatusViewModel Create(Status model, int i)
        {
            if (model.Entities.MediaEntities.Any())
            {
                return new PhotoStatusViewModel(model, i);
            }

            return new StatusViewModel(model, i);
        }
    }
}
