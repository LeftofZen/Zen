using Zenith.Colour;
using Zenith.Core;
using Zenith.Maths;
using Zenith.Maths.Points;
using Zenith.Maths.Vectors;

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

		public static ImageBuffer Convolve(this ImageBuffer buf, Kernel kernel, EdgeHandling edgeHandling)
		{
			Verify.Odd(kernel.Width);
			Verify.Odd(kernel.Height);

			var newBuf = new ImageBuffer(buf.Width, buf.Height);

			var centre = new Point2(kernel.Width / 2, kernel.Height / 2);
			var xOffset = edgeHandling == EdgeHandling.Crop ? centre.X : 0;
			var yOffset = edgeHandling == EdgeHandling.Crop ? centre.Y : 0;

			for (var y = 0 + yOffset; y < buf.Height - yOffset; ++y)
			{
				for (var x = 0 + xOffset; x < buf.Width - xOffset; ++x)
				{
					(var R, var G, var B) = buf.ApplyKernel(kernel, edgeHandling, centre, y, x);

					newBuf.SetPixel(x, y, new ColourRGB(
						Math.Clamp(R, 0, 1),
						Math.Clamp(G, 0, 1),
						Math.Clamp(B, 0, 1)));
				}
			}

			return newBuf;
		}

		private static (float R, float G, float B) ApplyKernel(this ImageBuffer buf, Kernel kernel, EdgeHandling edgeHandling, Point2 centre, int y, int x)
		{
			(float R, float G, float B) = (0, 0, 0);

			for (var ky = 0; ky < kernel.Height; ++ky)
			{
				for (var kx = 0; kx < kernel.Width; ++kx)
				{
					var nx = x + kx - centre.X;
					var ny = y + ky - centre.Y;

					if (edgeHandling == EdgeHandling.Skip)
					{
						if (!buf.IsValid(nx, ny))
						{
							continue;
						}
					}
					else if (edgeHandling == EdgeHandling.Wrap)
					{
						nx = (nx + buf.Width) % buf.Width;
						ny = (ny + buf.Height) % buf.Height;
					}
					else if (edgeHandling == EdgeHandling.Mirror)
					{
						if (nx < 0) nx *= -1;
						if (ny < 0) ny *= -1;
						if (nx >= buf.Width) nx = x - (kx - centre.X);
						if (ny >= buf.Height) ny = y - (ky - centre.Y);
					}
					else if (edgeHandling == EdgeHandling.Extend)
					{
						if (nx < 0) nx = 0;
						if (ny < 0) ny = 0;
						if (nx >= buf.Width) nx = buf.Width - 1;
						if (ny >= buf.Height) ny = buf.Height - 1;
					}

					var col = buf[nx, ny];
					var k = kernel[kx, ky];
					R += col.R * k;
					G += col.G * k;
					B += col.B * k;
				}
			}

			return (R, G, B);
		}

		static void Paste(this ImageBuffer buf, ImageBuffer src, int xOffset = 0, int yOffset = 0)
		{
			Verify.LessThanOrEqualTo(buf.Width, src.Width + xOffset);
			Verify.LessThanOrEqualTo(buf.Height, src.Height + yOffset);

			for (var y = yOffset; y < src.Height; ++y)
			{
				for (var x = xOffset; x < src.Width - xOffset; ++x)
				{
					buf[x, y] = src[x, y];
				}
			}
		}

		public static void Overlay(this ImageBuffer buf, ImageBuffer src, int xOffset = 0, int yOffset = 0, float blend = 1f)
		{
			Verify.LessThanOrEqualTo(buf.Width, src.Width + xOffset);
			Verify.LessThanOrEqualTo(buf.Height, src.Height + yOffset);
			Verify.InRangeInclusive(blend, 0f, 1f);

			// little optimisation to not have to iterate through every component if we're just blitting the image on top
			if (blend == 1f)
			{
				buf.Paste(src, xOffset, yOffset);
				return;
			}

			for (var y = yOffset; y < src.Height; ++y)
			{
				for (var x = xOffset; x < src.Width - xOffset; ++x)
				{
					buf[x, y] = MathsHelpers.Lerp<float, float, ColourRGB>(buf[x, y], src[x, y], blend);
				}
			}
		}
	}
}