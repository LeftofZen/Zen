using NUnit.Framework;
using Zenith.Colour;

namespace Zenith.Testing.Colour
{
	[TestFixture]
	public class ColourSpaceConverterTests
	{
		[Test]
		public void RGBtoHSB()
		{
			Assert.Multiple(() =>
			{
				var aquamarine = ColourSpaceConverter.RGBtoHSB(new ColourRGB { R = 0.5f, G = 1f, B = 1f });
				Assert.AreEqual(new ColourHSB { Hue = 0.5f, Saturation = 0.5f, Brightness = 1f }, aquamarine);

				var white = ColourSpaceConverter.RGBtoHSB(new ColourRGB { R = 1f, G = 1f, B = 1f });
				Assert.AreEqual(new ColourHSB { Hue = 0f, Saturation = 0f, Brightness = 1f }, white);

				var black = ColourSpaceConverter.RGBtoHSB(new ColourRGB { R = 0f, G = 0f, B = 0f });
				Assert.AreEqual(new ColourHSB { Hue = 0f, Saturation = 0f, Brightness = 0f }, black);

				// H and S can be anything in grayscale, only B affects RGB
				var greyish = ColourSpaceConverter.RGBtoHSB(new ColourRGB { R = 123 / 255f, G = 123 / 255f, B = 123 / 255f });
				Assert.AreEqual(0.48235294f, greyish.Brightness);

				var darkMagenta = ColourSpaceConverter.RGBtoHSB(new ColourRGB { R = 138 / 255f, G = 21 / 255f, B = 170 / 255f });
				Assert.AreEqual(new ColourHSB { Hue = 0.7975392f, Saturation = 0.87647057f, Brightness = 0.6666667f }, darkMagenta);
			});
		}

		[Test]
		public void HSBToRGB()
		{
			Assert.Multiple(() =>
			{
				var aquamarine = ColourSpaceConverter.HSBtoRGB(new ColourHSB { Hue = 0.5f, Saturation = 0.5f, Brightness = 1f });
				Assert.AreEqual(new ColourRGB { R = 0.5f, G = 1f, B = 1f }, aquamarine);

				var white = ColourSpaceConverter.HSBtoRGB(new ColourHSB { Hue = 0f, Saturation = 0f, Brightness = 1f });
				Assert.AreEqual(new ColourRGB { R = 1f, G = 1f, B = 1f }, white);

				var black = ColourSpaceConverter.HSBtoRGB(new ColourHSB { Hue = 0f, Saturation = 0f, Brightness = 0f });
				Assert.AreEqual(new ColourRGB { R = 0f, G = 0f, B = 0f }, black);

				var greyish = ColourSpaceConverter.HSBtoRGB(new ColourHSB { Hue = 0f, Saturation = 0f, Brightness = 0.48235294f });
				Assert.AreEqual(new ColourRGB { R = 123 / 255f, G = 123 / 255f, B = 123 / 255f }, greyish);
			});
		}

		[Test]
		public void IdempotenceRGB()
		{
			var rgb = new ColourRGB { R = 0.2f, G = 0.4f, B = 0.8f };
			var rgb2 = ColourSpaceConverter.HSBtoRGB(ColourSpaceConverter.RGBtoHSB(rgb));
			Assert.That(rgb.R, Is.EqualTo(rgb2.R).Within(0.0001f));
			Assert.That(rgb.G, Is.EqualTo(rgb2.G).Within(0.0001f));
			Assert.That(rgb.B, Is.EqualTo(rgb2.B).Within(0.0001f));
		}

		[Test]
		public void IdempotenceHSB()
		{
			var hsb = new ColourHSB { Hue = 0.12f, Saturation = 0.87f, Brightness = 0.48f };
			var hsb2 = ColourSpaceConverter.RGBtoHSB(ColourSpaceConverter.HSBtoRGB(hsb));
			Assert.That(hsb2.Hue, Is.EqualTo(hsb.Hue).Within(0.01f));
			Assert.That(hsb2.Saturation, Is.EqualTo(hsb.Saturation).Within(0.01f));
			Assert.That(hsb2.Brightness, Is.EqualTo(hsb.Brightness).Within(0.01f));
		}
	}
}