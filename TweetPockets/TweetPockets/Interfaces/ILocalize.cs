using System.Globalization;

namespace TweetPockets.Interfaces
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
    }
}