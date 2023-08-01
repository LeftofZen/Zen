using heightmap_gen.Algorithms;
using Zenith.Core;

namespace heightmap_gen.Generators
{
	public record DiamondSquareParams(
		int Width,
		int Height,
		long Seed = 0,
		double Roughness = 1.0,
		double? InitialValue = null);

	public static class DiamondSquareGenerator
	{
		public static double[,] Generate(DiamondSquareParams dsp)
		{
			var rnd = new Random((int)dsp.Seed);
			var initialValue = dsp.InitialValue is null or 0 ? rnd.NextDouble() : dsp.InitialValue.Value;

			var maxPoints = Math.Max(dsp.Width, dsp.Height);
			Verify.Positive(maxPoints);
			// diamond-square requires power-of-2 input sizes;
			// calculate the next highest power of 2 above the max of width and height
			var points = Math.Pow(2, Math.Ceiling(Math.Log2(maxPoints)));

			Console.WriteLine(initialValue);
			var ds = new DiamondSquare((int)points, dsp.Roughness, initialValue, rnd);
			var data = ds.GetData();
			data.Normalise();
			return data;
		}
	}
}