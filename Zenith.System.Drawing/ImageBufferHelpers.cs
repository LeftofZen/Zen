using System.Drawing.Imaging;
using System.Drawing;
using Zenith.Drawing;

namespace Zenith.System.Drawing
{
	public static class ImageBufferHelpers
	{
		public static ImageBuffer FromBitmap(Bitmap img)
		{
			var imageBuffer = new ImageBuffer(img.Width, img.Height);
			var rect = new Rectangle(0, 0, imageBuffer.Width, imageBuffer.Height);
			var imgData = img.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			for (var y = 0; y < imageBuffer.Height; ++y)
			{
				for (var x = 0; x < imageBuffer.Width; ++x)
				{
					var pixel = imgData.GetPixel(x, y);
					imageBuffer.SetPixel(x, y, pixel);
				}
			}

			img.UnlockBits(imgData);
			return imageBuffer;
		}
	}
}
