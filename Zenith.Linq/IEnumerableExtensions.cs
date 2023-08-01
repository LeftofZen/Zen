namespace Zenith.Linq
{
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Enumerates the list applying `action` to each item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="action"></param>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source)
			{
				action(item);
			}
		}

		/// <summary>
		/// Flattens an IEnumerable<(T, T)> into an IEnumerable<T>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns>An IEnumerable<T> containing each of the individual components of the pairs in the source list.</returns>
		public static IEnumerable<T> SelectManyTuples<T>(this IEnumerable<(T, T)> source)
		{
			foreach (var (item1, item2) in source)
			{
				yield return item1;
				yield return item2;
			}
		}

		public static IEnumerable<T> SelectManyTuples<T>(this IEnumerable<(T, T, T)> source)
		{
			foreach (var (item1, item2, item3) in source)
			{
				yield return item1;
				yield return item2;
				yield return item3;
			}
		}

		public static IEnumerable<T> SelectManyTuples<T>(this IEnumerable<(T, T, T, T)> source)
		{
			foreach (var (item1, item2, item3, item4) in source)
			{
				yield return item1;
				yield return item2;
				yield return item3;
				yield return item4;
			}
		}

		// ... etc

		/// <summary>
		/// Converts an IEnumerable<T> into a SortedSet<T>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Given a list of [A, B, C, D, ...], returns the interval pairs [(A, B), (B, C), (C, D), (D, ...)].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <returns>Interval pairs of the input sequence.</returns>
		public static IEnumerable<(T First, T Second)> Intervals<T>(this IEnumerable<T> source)
			=> source.Zip(source.Skip(1));
	}
}