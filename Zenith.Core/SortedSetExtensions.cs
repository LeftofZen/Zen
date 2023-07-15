using Zenith.Linq;

namespace Zenith.Core
{
	public static class SortedSetExtensions
	{
		public static void AddRange<T>(this SortedSet<T> source, IEnumerable<T> toAdd)
			=> toAdd.ForEach(t => source.Add(t));
	}
}
