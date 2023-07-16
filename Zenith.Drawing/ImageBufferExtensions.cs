using Zenith.Colour;
using Zenith.Maths.Points;

namespace Zenith.Drawing
{
	public static class ImageBufferExtensions
	{
		public static IEnumerable<Point2> GetNeighbourPoints(this ImageBuffer buf, Point2 p)
		{
			for (var x = -1; x < 2; ++x)
			{
				for (var y = -1; y < 2; ++y)
				{
					if (x != 0 || y != 0)
					{
						if (p.X + x >= 0 && p.X + x < buf.Width && p.Y + y >= 0 && p.Y + y < buf.Height)
						{
							yield return new Point2(p.X + x, p.Y + y);
						}
					}
				}
			}
		}

		public static IEnumerable<ColourRGB> GetEmptyNeighbourColours(this ImageBuffer buf, Point2 p)
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

		public static IEnumerable<Point2> GetEmptyNeighbourPoints(this ImageBuffer buf, Point2 p)
		{
			for (var x = -1; x < 2; ++x)
			{
				for (var y = -1; y < 2; ++y)
				{
					if (p.X + x >= 0 && p.X + x < buf.Width && p.Y + y >= 0 && p.Y + y < buf.Height)
					{
						if (buf.IsEmpty(p.X + x, p.Y + y))
						{
							yield return new Point2(p.X + x, p.Y + y);
						}
					}
				}
			}
		}
	}
}