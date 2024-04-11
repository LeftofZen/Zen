using Zenith.Algorithms;

namespace Zenith.ProceduralGeneration
{
	public static class SimplexNoiseGenerator
	{
		public static double[,] Generate(SimplexNoiseParams snp)
		{
			var noise = new OpenSimplexNoise(snp.Seed == 0 ? new Random().NextInt64() : snp.Seed);
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
}