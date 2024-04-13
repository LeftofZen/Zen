using NUnit.Framework;
using Zenith.Colour;
using Zenith.Drawing;

namespace Zenith.Testing.Drawing
{
	[TestFixture]
	public class ImageBufferExtensionsTests
	{
		[Test]
		public void TestConvolve()
		{
			var col = new ColourRGB[,]
			{
				{ ColourRGB.Black, ColourRGB.Cyan,ColourRGB.Green },
				{ ColourRGB.Magenta, ColourRGB.Red, ColourRGB.White },
				{ ColourRGB.Yellow, ColourRGB.Red, ColourRGB.Blue },
			};

			var buf = new ImageBuffer(col);

			Assert.Multiple(() =>
			{
				Assert.That(ColourRGB.Black, Is.EqualTo(buf[0, 0]));
				Assert.That(ColourRGB.Magenta, Is.EqualTo(buf[1, 0]));
				Assert.That(ColourRGB.Yellow, Is.EqualTo(buf[2, 0]));
				Assert.That(ColourRGB.Cyan, Is.EqualTo(buf[0, 1]));
				Assert.That(ColourRGB.Red, Is.EqualTo(buf[1, 1]));
				Assert.That(ColourRGB.Red, Is.EqualTo(buf[2, 1]));
				Assert.That(ColourRGB.Green, Is.EqualTo(buf[0, 2]));
				Assert.That(ColourRGB.White, Is.EqualTo(buf[1, 2]));
				Assert.That(ColourRGB.Blue, Is.EqualTo(buf[2, 2]));
			});

			buf.Convolve(Kernels.Identity, EdgeHandling.Crop);

			Assert.Multiple(() =>
			{
				Assert.That(ColourRGB.Black, Is.EqualTo(buf[0, 0]));
				Assert.That(ColourRGB.Magenta, Is.EqualTo(buf[1, 0]));
				Assert.That(ColourRGB.Yellow, Is.EqualTo(buf[2, 0]));
				Assert.That(ColourRGB.Cyan, Is.EqualTo(buf[0, 1]));
				Assert.That(ColourRGB.Red, Is.EqualTo(buf[1, 1]));
				Assert.That(ColourRGB.Red, Is.EqualTo(buf[2, 1]));
				Assert.That(ColourRGB.Green, Is.EqualTo(buf[0, 2]));
				Assert.That(ColourRGB.White, Is.EqualTo(buf[1, 2]));
				Assert.That(ColourRGB.Blue, Is.EqualTo(buf[2, 2]));
			});
		}
	}
}
