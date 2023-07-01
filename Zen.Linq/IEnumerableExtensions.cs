namespace Zen.Linq
{
	public static class IEnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source)
			{
				action(item);
			}
		}

		public static IEnumerable<T> SelectManyTuples<T>(this IEnumerable<(T Item1, T Item2)> source)
		{
			foreach (var (item1, item2) in source)
			{
				yield return item1;
				yield return item2;
			}
		}

		public static SortedSet<T> ToSortedSet<T>(this IEnumerable<T> source)
			=> new(source);

		/// <summary>
		/// The permutation of 2 IEnumerables or the cross-product.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="other"></param>
		/// <returns>All pairs of items of the two input lists.</returns>
		public static IEnumerable<(T First, T Second)> Permute<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			foreach (var v in source)
			{
				foreach (var w in other)
				{
					yield return (v, w);
				}
			}
		}
	}
}