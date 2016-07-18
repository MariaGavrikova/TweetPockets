using Object = Java.Lang.Object;

namespace TweetPockets.Droid.Utilities
{
	public class JavaObjectWrapper<T> : Object
	{
		public readonly T Value;

		public JavaObjectWrapper (T value)
		{
			this.Value = value; 
		}
	}
}

