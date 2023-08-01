using Zenith.Algorithms;

namespace Zenith.ProceduralGeneration
{
	public static class SimplexNoiseGenerator
	{
		public static double[,] Generate(SimplexNoiseParams snp)
		{
			var noise = new OpenSimplexNoise(snp.Seed == 0 ? snp.Seed : new Random().NextInt64());
			var data = new double[snp.Width, snp.Height];

			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					var amplitude = (double)snp.InitialAmplitude;
					var frequency = (double)snp.InitialFrequency;
					var totalAmplitude = 0.0;
					var total = 0.0;
					var xEval = x + snp.XOffset;
					var yEval = y + snp.YOffset;

					for (var o = 0; o < snp.Octaves; o++)
					{
						var noisev = noise.Evaluate(xEval * frequency, yEval * frequency);

						// [[-1, 1] -> [0, 1]
						noisev = (noisev + 1) / 2;
						noisev *= amplitude;
						total += noisev;

						totalAmplitude += amplitude;
						amplitude *= snp.Persistence;
						frequency *= snp.Lacunarity;
					}

					total = Math.Pow(total, snp.Redistribution);

					// normalise
					total /= totalAmplitude;

					data[x, y] = total;
				}
			}

			if (snp.NormaliseOutput)
			{
				data.Normalise();
			}

			if (snp.TerraceCount > 1)
			{
				data.Terrace(snp.TerraceCount);
			}

			return data;
		}
	}

	public static class NoiseHelpers
	{
		public static void Terrace(this double[,] data, int count)
		{
			var min = double.MaxValue;
			var max = double.MinValue;
			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					min = Math.Min(min, data[x, y]);
					max = Math.Max(max, data[x, y]);
				}
			}

			var range = max - min;
			var terraceHeight = range / (count - 1); // -1 because there's an implicit extra terrace at the end of the range

			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					var terraceNumber = (int)((data[x, y] - min) / terraceHeight);
					data[x, y] = min + (terraceNumber * terraceHeight);
				}
			}
		}

		public static void Normalise(this double[,] data)
		{
			var min = double.MaxValue;
			var max = double.MinValue;
			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					min = Math.Min(min, data[x, y]);
					max = Math.Max(max, data[x, y]);
				}
			}

			var range = max - min;
			var xx = 1f / range;

			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					data[x, y] = (data[x, y] - min) * xx;
				}
			}
		}
	}
}