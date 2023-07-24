using Zenith.Colour;
using Zenith.Maths;
using Zenith.Maths.Points;
using Zenith.Core;

namespace Zenith.Drawing
{
	public class ImageBuffer
	{
		private ColourRGB[,] buf;
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
			buf = new ColourRGB[height, width];
			isSet = new bool[height, width];
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
		{
			buf[Y, X] = c;
			isSet[Y, X] = true;
		}

		public bool IsEmpty(Point2 p)
			=> IsEmpty(p.X, p.Y);

		public bool IsEmpty(int X, int Y)
			=> !isSet[Y, X];

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
	}
}