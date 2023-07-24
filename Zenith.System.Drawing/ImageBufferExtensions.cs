using System.Drawing.Imaging;
using System.Drawing;
using Zenith.Drawing;

namespace Zenith.System.Drawing
{
	public static class ImageBufferExtensions
	{
		public static Image GetImage(this ImageBuffer imageBuffer)
		{
			if (imageBuffer.Width <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(imageBuffer.Width), imageBuffer.Width, "Image width was invalid.");
			}

			if (imageBuffer.Height <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(imageBuffer.Height), imageBuffer.Height, "Image height was invalid.");
			}

			var img = new Bitmap(imageBuffer.Width, imageBuffer.Height, PixelFormat.Format32bppArgb);
			var rect = new Rectangle(0, 0, imageBuffer.Width, imageBuffer.Height);
			var imgData = img.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			for (var y = 0; y < imageBuffer.Height; ++y)
			{
				for (var x = 0; x < imageBuffer.Width; ++x)
				{
					imgData.SetPixel(x, y, !imageBuffer.IsEmpty(x, y) ? imageBuffer.GetPixel(x, y) : Color.White.ToColourRGB());
				}
			}

			img.UnlockBits(imgData);
			return img;
		}

		public static void Save(this ImageBuffer imageBuffer, string directory)
			=> Save(imageBuffer.GetImage(), directory);

		private static void Save(Image image, string directory)
		{
			Console.WriteLine("Saving");
			var filename = @$"{directory}\img_{DateTime.Now.ToString().Replace(':', '-')}_{image.Width}x{image.Height}.png";
			filename = filename.Replace(' ', '_');
			image.Save(filename, ImageFormat.Png);
		}
	}
}
