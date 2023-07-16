using NUnit.Framework;
using Zenith.Colour;

namespace Zenith.Testing.Colour
{
	[TestFixture]
	public class ColourGeneratorTests
	{
		[Test]
		public void TestColourGenerator()
		{
			var colours = ColourGenerator.GenerateColours_RGB_Uniform(3 * 3 * 3);
			Assert.AreEqual(27, colours.Count);

			Assert.That(colours.Contains(new ColourRGB(0f, 0f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(0f, 0f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(0f, 0f, 1f)));
			Assert.That(colours.Contains(new ColourRGB(0f, 0.5f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(0f, 0.5f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(0f, 0.5f, 1f)));
			Assert.That(colours.Contains(new ColourRGB(0f, 1f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(0f, 1f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(0f, 1f, 1f)));

			Assert.That(colours.Contains(new ColourRGB(0.5f, 0f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(0.5f, 0f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(0.5f, 0f, 1f)));
			Assert.That(colours.Contains(new ColourRGB(0.5f, 0.5f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(0.5f, 0.5f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(0.5f, 0.5f, 1f)));
			Assert.That(colours.Contains(new ColourRGB(0.5f, 1f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(0.5f, 1f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(0.5f, 1f, 1f)));

			Assert.That(colours.Contains(new ColourRGB(1f, 0f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(1f, 0f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(1f, 0f, 1f)));
			Assert.That(colours.Contains(new ColourRGB(1f, 0.5f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(1f, 0.5f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(1f, 0.5f, 1f)));
			Assert.That(colours.Contains(new ColourRGB(1f, 1f, 0f)));
			Assert.That(colours.Contains(new ColourRGB(1f, 1f, 0.5f)));
			Assert.That(colours.Contains(new ColourRGB(1f, 1f, 1f)));
		}
	}
}
