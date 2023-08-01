using Zenith.Core;

namespace Zenith.Drawing
{
	// https://en.wikipedia.org/wiki/Kernel_(image_processing)#Edge_handling
	public enum EdgeHandling
	{
		Crop, KernelCrop, Wrap, Mirror, Skip, Extend
	}

	public class Kernel : Array2D<float>
	{
		public Kernel(float[,] data) : base(data)
		{
		}

		public Kernel(float[,] data, float scale) : base(data)
		{
			for (var y = 0; y < Height; ++y)
			{
				for (var x = 0; x < Width; ++x)
				{
					Data[x, y] *= scale;
				}
			}
		}
	}

	// https://en.wikipedia.org/wiki/Kernel_(image_processing)#Details
	public static class Kernels
	{
		public static readonly Kernel Identity = new(
			new float[,]
			{
				{ 0, 0, 0 },
				{ 0, 1, 0 },
				{ 0, 0, 0 },
			});

		public static readonly Kernel Blur = new(
			new float[,]
			{
				{ 1, 1, 1 },
				{ 1, 1, 1 },
				{ 1, 1, 1 },
			}, 1 / 9f);

		public static readonly Kernel Sharpen = new(
			new float[,]
			{
				{ 0, -1, 0 },
				{ -1, 5, -1 },
				{ 0, -1, 0 },
			});

		public static readonly Kernel EdgeDetection = new(
			new float[,]
			{
				{ 0, -1, 0 },
				{ -1, 4, -1 },
				{ 0 , -1, 0 },
			});

		public static readonly Kernel EdgeDetection2 = new(
			new float[,]
			{
				{ -1, -1, -1 },
				{ -1,  8, -1 },
				{ -1, -1, -1 },
			});

		public static readonly Kernel GaussianBlur3x3 = new(
			new float[,]
			{
				{ 1, 2, 1 },
				{ 2, 4, 2 },
				{ 1, 2, 1 },
			}, 1 / 16f);

		public static readonly Kernel GaussianBlur5x5 = new(
			new float[,]
			{
				{ 1, 4, 6, 4, 1 },
				{ 4, 16, 24, 16, 4 },
				{ 6, 24, 36, 24, 6 },
				{ 4, 16, 24, 16, 4 },
				{ 1, 4, 6, 4, 1 },
			}, 1 / 256f);

		public static readonly Kernel UnsharpMask = new(
			new float[,]
			{
				{ 1, 4, 6, 4, 1 },
				{ 4, 16, 24, 16, 4 },
				{ 6, 24, -476, 24, 6 },
				{ 4, 16, 24, 16, 4 },
				{ 1, 4, 6, 4, 1 },
			}, -1 / 256f);
	}
}