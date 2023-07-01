namespace Zen.Core
{
	public static class DictionaryExtensions
	{
		public static void AddOrInsert<TKey, TValue>(this IDictionary<TKey, IList<TValue>> source, TKey key, TValue value)
		{
			if (!source.ContainsKey(key))
			{
				source.Add(key, new List<TValue>());
			}

			source[key].Add(value);
		}
	}
}
