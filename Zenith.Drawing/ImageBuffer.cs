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
		public ImageBuffer(int width, int height)
		{
			buf = new ColourRGB[height, width];
			buf.Fill(ColourRGB.None);

			isSet = new bool[height, width];
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

		private ColourRGB[,] buf;
		private bool[,] isSet;

		public void Clear()
		{
			buf = new ColourRGB[Height, Width];
			isSet = new bool[Height, Width];
		}

		public bool Contains(int X, int Y)
			=> X >= 0 && X < Width && Y >= 0 && Y < Height;

		public bool Contains(Point p)
			=> Contains(p.X, p.Y);

		public ColourRGB GetPixel(Point p)
			=> GetPixel(p.X, p.Y);
		public ColourRGB GetPixel(int X, int Y)
			=> buf[Y, X];

		public void SetPixel(Point p, ColourRGB c)
			=> SetPixel(p.X, p.Y, c);

		public void SetPixel(int X, int Y, ColourRGB c)
		{
			isSet[Y, X] = true;
			buf[Y, X] = c;
		}

		public bool IsEmpty(Point p)
			=> IsEmpty(p.X, p.Y);

		public bool IsEmpty(int X, int Y)
			//=> !isSet[Y, X];
			=> buf[Y, X] == ColourRGB.None;

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
					imgData.SetPixel(x, y, isSet[y, x] ? GetPixel(x, y) : Color.White.ToColourRGB());
				}
			}

			img.UnlockBits(imgData);
			return img;
		}

		public void Save()
			=> Save(GetImage());

		private void Save(Image i)
		{
			Console.WriteLine("Saving");
			//i.Save(@"C:\Users\Benjamin.Sutas\source\repos\all-rgb\all-rgb\content\img.png", ImageFormat.Png);
			var filename = @$"{BaseFileName}\img_{DateTime.Now.ToString().Replace(':', '-')}_{Width}x{Height}.png";
			filename = filename.Replace(' ', '_');
			i.Save(filename, ImageFormat.Png);
		}

		public const string BaseFileName = @"C:\Users\bigba\source\repos\all-rgb\all-rgb\content";
	}

	public static class ImageBufferExtensions
	{
		public static IEnumerable<Point> GetNeighbourPoints(this ImageBuffer buf, Point p)
		{
			for (var x = -1; x < 2; ++x)
			{
				for (var y = -1; y < 2; ++y)
				{
					if (x != 0 || y != 0)
					{
						if (p.X + x >= 0 && p.X + x < buf.Width && p.Y + y >= 0 && p.Y + y < buf.Height)
						{
							yield return new Point(p.X + x, p.Y + y);
						}
					}
				}
			}
		}

		public static IEnumerable<ColourRGB> GetNonEmptyNeighbourColours(this ImageBuffer buf, Point p)
		{
			for (var x = -1; x < 2; ++x)
			{
				for (var y = -1; y < 2; ++y)
				{
					if (p.X + x >= 0 && p.X + x < buf.Width && p.Y + y >= 0 && p.Y + y < buf.Height)
					{
						if (buf.IsEmpty(p.X + x, p.Y + y))
						{
							yield return buf.GetPixel(p.X + x, p.Y + y);
						}
					}
				}
			}
		}

		public static IEnumerable<Point> GetNonEmptyNeighbourPoints(this ImageBuffer buf, Point p)
		{
			for (var x = -1; x < 2; ++x)
			{
				for (var y = -1; y < 2; ++y)
				{
					if (p.X + x >= 0 && p.X + x < buf.Width && p.Y + y >= 0 && p.Y + y < buf.Height)
					{
						if (buf.IsEmpty(p.X + x, p.Y + y))
						{
							yield return new Point(p.X + x, p.Y + y);
						}
					}
				}
			}
		}
	}
}