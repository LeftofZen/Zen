using NUnit.Framework;
using Zenith.Linq;

namespace Zenith.Testing.Linq
{
	[TestFixture]
	public class IListExtensionsTests
	{
		[Test]
		public void Shuffle()
		{
			var input = new List<int>() { 1, 2, 3, 4, 5, 6 };
			input.Shuffle();

			// assumes the seed of `rnd` in IListExtensions is 1
			Assert.Multiple(() =>
			{
				Assert.AreEqual(2, input[0]);
				Assert.AreEqual(1, input[1]);
				Assert.AreEqual(4, input[2]);
				Assert.AreEqual(6, input[3]);
				Assert.AreEqual(3, input[4]);
				Assert.AreEqual(5, input[5]);
			});
		}
	}
}
