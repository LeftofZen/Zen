namespace Zenith.Linq
{
	public static class IListExtensions
	{
		static readonly Random rnd = new(1);

		/// <summary>
		/// Fisher-Yates shuffle with added percentage and skipping inputs.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="percentToDeviate">The maximum distance an item will be shuffled to.</param>
		/// <param name="skip">How many items in the original list we apply the shuffle to, versus how many we omit from the shuffle or 'skip'. In other words, skipping an item means it will remain it ints original position in the list.</param>
		public static void Shuffle<T>(this IList<T> source, float percentToDeviate = 1f, int skip = 1)
		{
			for (var i = 0; i < source.Count - 1; i += skip)
			{
				var r = rnd.Next(i, (int)(((source.Count - i) * percentToDeviate) + i));
				(source[r], source[i]) = (source[i], source[r]);
			}
		}

		/// <summary>
		/// Fisher-Yates shuffle
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		public static void Shuffle<T>(this IList<T> source)
		{
			for (var i = 0; i < source.Count - 1; ++i)
			{
				var r = rnd.Next(i, source.Count);
				(source[r], source[i]) = (source[i], source[r]);
			}
		}
	}
}