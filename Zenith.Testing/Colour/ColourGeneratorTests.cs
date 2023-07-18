using NUnit.Framework;
using Zenith.Colour;

namespace Zenith.Testing.Colour
{
	[TestFixture]
	public class ColourGeneratorTests
	{
		[Test]
		[TestCase(128 * 128)]
		[TestCase(256 * 256)]
		[TestCase(2560 * 1440)]
		[TestCase(4096 * 4096)]
		public void ColourGeneratorCounts(int pixels)
		{
			var coloursRGB = ColourGenerator.GenerateColours_RGB_Uniform(pixels);
			Assert.AreEqual(pixels, coloursRGB.Count);

			var coloursHSB = ColourGenerator.GenerateColours_HSB_Uniform(pixels);
			Assert.AreEqual(pixels, coloursHSB.Count);
		}

		[Test]
		public void GenerateRGB()
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

		[Test]
		public void ColourGeneratorFull()
		{
			var rgbColours = ColourGenerator.GenerateColours_RGB_All().ToList();
			var hsbColours = rgbColours.Select(c => c.AsHSB()).Distinct().ToList();
			var rgbColours2 = hsbColours.Select(c => c.AsRGB()).Distinct().ToList();
			Assert.AreEqual(rgbColours.Count, hsbColours.Count);
			Assert.AreEqual(hsbColours.Count, rgbColours2.Count);
		}
	}
}
