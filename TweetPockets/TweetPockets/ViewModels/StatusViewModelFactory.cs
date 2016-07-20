using System.Linq;
using LinqToTwitter;

namespace TweetPockets.ViewModels
{
    class StatusViewModelFactory
    {
        public StatusViewModel Create(Status model)
        {
            if (model.Entities.MediaEntities.Any())
            {
                return new PhotoStatusViewModel(model);
            }

            return new StatusViewModel(model);
        }
    }
}
