using NUnit.Framework;
using Zenith.Colour;

namespace Zenith.Testing.Colour
{
	[TestFixture]
	public class ColourGeneratorTests
	{
		[Test]
		public void GenerateHSB()
		{
			var colours = ColourGenerator.GenerateColours_HSB_Random(128);
			Assert.That(colours.Select(c => c.Hue), Is.All.InRange(0f, 1f));
			Assert.That(colours.Select(c => c.Saturation), Is.All.InRange(0f, 1f));
			Assert.That(colours.Select(c => c.Brightness), Is.All.InRange(0f, 1f));
		}
	}
}
