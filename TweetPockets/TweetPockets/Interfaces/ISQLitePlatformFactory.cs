using SQLite.Net.Interop;

namespace TweetPockets.Interfaces
{
    public interface ISQLitePlatformFactory
    {
        ISQLitePlatform CreatePlatform();

        string CreateDatabaseFile(string fileName);
    }
}