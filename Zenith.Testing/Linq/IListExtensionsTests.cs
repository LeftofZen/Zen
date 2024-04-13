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
				Assert.That(2, Is.EqualTo(input[0]));
				Assert.That(1, Is.EqualTo(input[1]));
				Assert.That(4, Is.EqualTo(input[2]));
				Assert.That(6, Is.EqualTo(input[3]));
				Assert.That(3, Is.EqualTo(input[4]));
				Assert.That(5, Is.EqualTo(input[5]));
			});
		}
	}
}
