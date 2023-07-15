using NUnit.Framework;

namespace Zenith.Colour.Testing
{
	[TestFixture]
	public class ColourEqualiserTest
	{
		[Test]
		public void TestEqualise()
		{
			// arrange
			var colours = new HashSet<ColourRGB>();
			_ = colours.Add(new ColourRGB(21, 49, 136));
			_ = colours.Add(new ColourRGB(255, 19, 54));
			_ = colours.Add(new ColourRGB(11, 194, 236));
			_ = colours.Add(new ColourRGB(0, 255, 13));

			// act
			var result = ColourEqualiser.Equalise(colours);

			// assert
			Assert.AreEqual(0, result.Min(c => c.R));
			Assert.AreEqual(0, result.Min(c => c.G));
			Assert.AreEqual(0, result.Min(c => c.B));

			Assert.AreEqual(255, result.Max(c => c.R));
			Assert.AreEqual(255, result.Max(c => c.G));
			Assert.AreEqual(255, result.Max(c => c.B));
		}
	}
}
