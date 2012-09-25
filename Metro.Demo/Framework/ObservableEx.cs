namespace Metro.Framework
{
	using System;
	using System.Linq;

	/// <summary>
	/// Extension methods for IObservable.
	/// </summary>
	/// <remarks>http://tozon.info/blog/post/2010/10/06/Lost-in-time-Zip-it!.aspx</remarks>
	public static class ObservableEx
	{
		public static IObservable<TSource> OnTimeline<TSource>(this IObservable<TSource> source, TimeSpan period)
		{
			return source.Zip(Observable.Interval(period), (d, t) => d);
		}
	}
}