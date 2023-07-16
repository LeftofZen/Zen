using NUnit.Framework;
using Zenith.Colour;

namespace Zenith.Testing.Colour
{
	[TestFixture]
	public class ColourTests
	{
		[Test]
		public void TestRGB()
		{
			// positive out of range
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourRGB(1.01f, 0.01f, 0.01f));
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourRGB(0.01f, 1.01f, 0.01f));
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourRGB(0.01f, 0.01f, 1.01f));

			// negative out of range
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourRGB(-0.01f, 0.01f, 0.01f));
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourRGB(0.01f, -0.01f, 0.01f));
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourRGB(0.01f, 0.01f, -0.01f));
		}

		[Test]
		public void TestHSB()
		{
			// positive out of range
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourHSB(1.01f, 0.01f, 0.01f));
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourHSB(0.01f, 1.01f, 0.01f));
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourHSB(0.01f, 0.01f, 1.01f));

			_ = // negative out of range
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourHSB(-0.01f, 0.01f, 0.01f));
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourHSB(0.01f, -0.01f, 0.01f));
			_ = Assert.Throws<ArgumentOutOfRangeException>(() => new ColourHSB(0.01f, 0.01f, -0.01f));
		}
	}
}
