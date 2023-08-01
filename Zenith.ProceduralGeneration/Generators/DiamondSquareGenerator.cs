using Zenith.Algorithms;
using Zenith.Core;

namespace Zenith.ProceduralGeneration
{
	public static class DiamondSquareGenerator
	{
		public static double[,] Generate(DiamondSquareParams dsp)
		{
			var rnd = new Random(dsp.Seed == 0 ? new Random().Next() : (int)dsp.Seed);
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