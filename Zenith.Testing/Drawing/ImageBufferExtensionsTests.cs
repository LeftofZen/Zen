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
				Assert.AreEqual(ColourRGB.Black, buf[0, 0]);
				Assert.AreEqual(ColourRGB.Magenta, buf[1, 0]);
				Assert.AreEqual(ColourRGB.Yellow, buf[2, 0]);
				Assert.AreEqual(ColourRGB.Cyan, buf[0, 1]);
				Assert.AreEqual(ColourRGB.Red, buf[1, 1]);
				Assert.AreEqual(ColourRGB.Red, buf[2, 1]);
				Assert.AreEqual(ColourRGB.Green, buf[0, 2]);
				Assert.AreEqual(ColourRGB.White, buf[1, 2]);
				Assert.AreEqual(ColourRGB.Blue, buf[2, 2]);
			});

			buf.Convolve(Kernels.Identity, EdgeHandling.Crop);

			Assert.Multiple(() =>
			{
				Assert.AreEqual(ColourRGB.Black, buf[0, 0]);
				Assert.AreEqual(ColourRGB.Magenta, buf[1, 0]);
				Assert.AreEqual(ColourRGB.Yellow, buf[2, 0]);
				Assert.AreEqual(ColourRGB.Cyan, buf[0, 1]);
				Assert.AreEqual(ColourRGB.Red, buf[1, 1]);
				Assert.AreEqual(ColourRGB.Red, buf[2, 1]);
				Assert.AreEqual(ColourRGB.Green, buf[0, 2]);
				Assert.AreEqual(ColourRGB.White, buf[1, 2]);
				Assert.AreEqual(ColourRGB.Blue, buf[2, 2]);
			});
		}
	}
}
