using Zenith.Colour;
using Zenith.Maths;
using Zenith.Maths.Points;
using Zenith.Core;

namespace Zenith.Drawing
{
	public class ImageBuffer
	{
		private ColourRGB[,] buf; // make some Pixel : IVector3<int> class in future?
		private bool[,] isSet;

#pragma warning disable CS8618 // Bogus error - Clear() method sets buf and isSet
		public ImageBuffer(int width, int height)
#pragma warning restore CS8618 // Bogus error - Clear() method sets buf and isSet
		{
			Clear(width, height);
			Radius = MathsHelpers.Distance.Euclidean(Point2.Zero, Middle);
		}

		public float Radius { get; init; }

		public void Clear()
			=> Clear(Width, Height);

		private void Clear(int width, int height)
		{
			buf = new ColourRGB[width, height];
			isSet = new bool[width, height];
		}

		public bool Contains(int X, int Y)
			=> X >= 0 && X < Width && Y >= 0 && Y < Height;

		public bool Contains(Point2 p)
			=> Contains(p.X, p.Y);

		public ColourRGB GetPixel(Point2 p)
			=> GetPixel(p.X, p.Y);

		public ColourRGB GetPixel(int X, int Y)
			=> buf[X, Y];

		public void SetPixel(Point2 p, ColourRGB c)
			=> SetPixel(p.X, p.Y, c);

		public void SetPixel(int X, int Y, ColourRGB c)
		{
			buf[X, Y] = c;
			isSet[X, Y] = true;
		}

		public bool IsEmpty(Point2 p)
			=> IsEmpty(p.X, p.Y);

		public bool IsEmpty(int X, int Y)
			=> !isSet[X, Y];

		public void Fill(ColourRGB fillColour)
			=> buf.Fill(fillColour);

		public void FillRect(int X, int Y, int Width, int Height, ColourRGB c)
		{
			for (var y = Y; y < Y + Height; ++y)
			{
				for (var x = X; x < X + Width; ++x)
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
	}
}