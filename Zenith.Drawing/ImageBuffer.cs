using System.Drawing;
using Zenith.Colour;
using System.Drawing.Imaging;
using Zenith.Maths;
using Zenith.Maths.Points;
using Zenith.System.Drawing;
using Zenith.Core;

namespace Zenith.Drawing
{
	public class ImageBuffer
	{
		private ColourRGB[,] buf;

		public ImageBuffer(int width, int height)
		{
			Clear(width, height);
			Radius = MathsHelpers.Distance.Euclidean(Point2.Zero, Middle);
		}

		public float Radius { get; init; }

		public ImageBuffer(Bitmap img) : this(img.Width, img.Height)
		{
			var rect = new Rectangle(0, 0, Width, Height);
			var imgData = img.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			for (var y = 0; y < Height; ++y)
			{
				for (var x = 0; x < Width; ++x)
				{
					SetPixel(x, y, imgData.GetPixel(x, y));
				}
			}

			img.UnlockBits(imgData);
		}
		public void Clear()
			=> Clear(Width, Height);

		private void Clear(int width, int height)
		{
			buf = new ColourRGB[height, width];
			Fill(ColourRGB.None);
		}

		public bool Contains(int X, int Y)
			=> X >= 0 && X < Width && Y >= 0 && Y < Height;

		public bool Contains(Point2 p)
			=> Contains(p.X, p.Y);

		public ColourRGB GetPixel(Point2 p)
			=> GetPixel(p.X, p.Y);

		public ColourRGB GetPixel(int X, int Y)
			=> buf[Y, X];

		public void SetPixel(Point2 p, ColourRGB c)
			=> SetPixel(p.X, p.Y, c);

		public void SetPixel(int X, int Y, ColourRGB c)
			=> buf[Y, X] = c;

		public bool IsEmpty(Point2 p)
			=> IsEmpty(p.X, p.Y);

		public bool IsEmpty(int X, int Y)
			=> buf[Y, X] == ColourRGB.None;

		public void Fill(ColourRGB fillColour)
			=> buf.Fill(fillColour);

		public void FillRect(int X, int Y, int Width, int Height, ColourRGB c)
		{
			for (var x = X; x < X + Width; ++x)
			{
				for (var y = Y; y < Y + Height; ++y)
				{
					SetPixel(x, y, c);
				}
			}
		}

		public int Width
			=> buf.GetLength(1);
		public int Height
			=> buf.GetLength(0);

		public int NumberOfPixels
			=> Width * Height;

		public Point2 Middle
			=> new(Width / 2, Height / 2);

		public Image GetImage()
		{
			if (Width == 0 || Height == 0)
			{
				throw new ArgumentOutOfRangeException("Width/Height", $"Image dimensions were invalid. Width={Width} Height={Height}");
			}

			var img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
			var rect = new Rectangle(0, 0, Width, Height);
			var imgData = img.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			for (var y = 0; y < Height; ++y)
			{
				for (var x = 0; x < Width; ++x)
				{
					imgData.SetPixel(x, y, !IsEmpty(x, y) ? GetPixel(x, y) : Color.White.ToColourRGB());
				}
			}

			img.UnlockBits(imgData);
			return img;
		}

		public void Save(string path)
			=> Save(GetImage(), path);

		private void Save(Image i, string path)
		{
			Console.WriteLine("Saving");
			var filename = @$"{path}\img_{DateTime.Now.ToString().Replace(':', '-')}_{Width}x{Height}.png";
			filename = filename.Replace(' ', '_');
			i.Save(filename, ImageFormat.Png);
		}
	}
}