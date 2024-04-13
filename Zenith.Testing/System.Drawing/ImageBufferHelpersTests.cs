using System.Drawing;
using NUnit.Framework;
using Zenith.Colour;
using Zenith.Drawing;
using Zenith.System.Drawing;

namespace Zenith.Testing.System.Drawing
{
	[TestFixture]
	public class ImageBufferHelpersTests
	{
		[Test]
		public void TestImageBufferSaveAndLoad()
		{
			const int width = 16;
			const int height = 16;
			var buf = GetRandomImageBuffer(width, height);
			var dir = TestContext.CurrentContext.WorkDirectory;
			var filename = buf.Save(dir);
			var newBuf = ImageBufferHelpers.FromBitmap(new Bitmap(filename));

			Assert.Multiple(() =>
			{
				for (var y = 0; y < height; ++y)
				{
					for (var x = 0; x < width; ++x)
					{
						Assert.That(buf.GetPixel(x, y).AsPixel(), Is.EqualTo(newBuf.GetPixel(x, y).AsPixel()), $"[{x}, {y}]");
					}
				}
			});
		}

		static Random rnd = new(1);

		public static ImageBuffer GetRandomImageBuffer(int width, int height)
		{
			var buf = new ImageBuffer(width, height);
			for (var y = 0; y < height; ++y)
			{
				for (var x = 0; x < width; ++x)
				{
					buf.SetPixel(x, y, GetRandomColour());
				}
			}
			return buf;
		}

		public static ColourRGB GetRandomColour()
			=> new((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
	}
}
