using heightmap_gen.Algorithms;

namespace heightmap_gen.Generators
{
	public record SimplexNoiseParams(
		int Width,
		int Height,
		long Seed = 0,
		double XOffset = 0,
		double YOffset = 0,
		int Octaves = 8,
		double Lacunarity = 3.0,
		double Persistence = 0.5,
		double InitialAmplitude = 1,
		double InitialFrequency = 0.005,
		double Redistribution = 1,
		bool UseTerracing = false,
		int TerraceCount = 10,
		bool NormaliseOutput = true);

	public static class SimplexNoiseGenerator
	{
		public static double[,] Generate(SimplexNoiseParams snp)
		{
			var noise = new OpenSimplexNoise(snp.Seed);
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

					//terraces
					if (snp.UseTerracing)
					{
						total = Math.Round(total * snp.TerraceCount);
					}

					// normalise
					total /= totalAmplitude;

					data[x, y] = total;
				}
			}

			if (snp.NormaliseOutput)
			{
				data.Normalise();
			}

			return data;
		}
	}

	public static class NoiseHelpers
	{
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