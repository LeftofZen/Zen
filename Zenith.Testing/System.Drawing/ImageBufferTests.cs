using NUnit.Framework;
using Zenith.System.Drawing;

namespace Zenith.Testing.System.Drawing
{
	[TestFixture]
	public class ImageBufferTests
	{
		[Test]
		public void IsEmpty()
		{
			var buf = new ImageBuffer(8, 8);

			for (var x = 0; x < buf.Width; ++x)
			{
				for (var y = 0; y < buf.Height; ++y)
				{
					Assert.True(buf.IsEmpty(x, y));
				}
			}
		}
	}
}
