using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace TweetPockets
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
