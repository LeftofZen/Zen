using System.Drawing.Imaging;
using System.Drawing;
using Zen.Colour;

namespace Zen.System.Drawing
{
	public static class BitmapDataExtensions
	{
		//static unsafe bool IsPixelSet(Color[,] d, Point p) => IsPixelSet(d, (p.X, p.Y));

		//static unsafe bool IsPixelSet(Color[,] d, (int X, int Y) p)
		//{
		//	if (p.X < 0 || p.Y < 0 || p.X >= d.Width || p.Y >= d.Height)
		//	{
		//		return false;
		//	}

		//	var ptr = (byte*)d.Scan0;
		//	var offset = (p.X + (p.Y * d.Height)) * 4;
		//	ptr += offset;
		//	return ptr[3] != 0; // alpha == 0 means unset
		//}

		//public static unsafe Color GetPixel(this BitmapData d, Point p)
		//	=> GetPixel(d, p.X, p.Y);

		public static unsafe ColourRGB GetPixel(this BitmapData d, int X, int Y)
		{
			var ptr = GetPtrToFirstPixel(d, X, Y);
			return new() { R = (int)(ptr[2] / 255f), G = (int)(ptr[1] / 255f), B = (int)(ptr[0] / 255f) }; // alpha is ptr[3]);
		}

		public static unsafe void SetPixel(this BitmapData d, Point p, ColourRGB c)
			=> SetPixel(d, p.X, p.Y, c);

		public static unsafe void SetPixel(this BitmapData d, int X, int Y, ColourRGB c)
			=> SetPixel(GetPtrToFirstPixel(d, X, Y), c);

		private static unsafe byte* GetPtrToFirstPixel(this BitmapData d, int X, int Y)
			=> (byte*)d.Scan0.ToPointer() + (Y * d.Stride) + (X * (Image.GetPixelFormatSize(d.PixelFormat) / 8));

		//private static unsafe void SetPixel(byte* ptr, Color c)
		//{
		//	ptr[0] = c.B; // Blue
		//	ptr[1] = c.G; // Green
		//	ptr[2] = c.R; // Red
		//	ptr[3] = c.A; // Alpha
		//}

		private static unsafe void SetPixel(byte* ptr, ColourRGB c)
		{
			ptr[0] = (byte)(c.B * 255); // Blue
			ptr[1] = (byte)(c.G * 255); // Green
			ptr[2] = (byte)(c.R * 255); // Red
			ptr[3] = 255; // (byte)(c.A * 255); // Alpha
		}
	}
}
