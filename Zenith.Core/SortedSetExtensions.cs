namespace Zenith.Core
{
	public static class SortedSetExtensions
	{
		public static bool AddRange<T>(this SortedSet<T> source, IEnumerable<T> toAdd)
		{
			var result = true;
			foreach (var v in toAdd)
			{
				result &= source.Add(v);
			}
			return result;
		}
	}
}
