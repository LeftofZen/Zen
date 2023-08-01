namespace Zenith.ProceduralGeneration
{
	// didn't use records here because records don't bind to propertygrid in winforms :|
	public class SimplexNoiseParams
	{
		public SimplexNoiseParams(
			int width,
			int height,
			long seed = 0,
			double xOffset = 0,
			double yOffset = 0,
			int octaves = 8,
			double lacunarity = 3.0,
			double persistence = 0.5,
			double initialAmplitude = 1.0,
			double initialFrequency = 0.005,
			double redistribution = 1.0,
			int terraceCount = 10,
			bool normaliseOutput = true)
		{
			Width = width;
			Height = height;
			Seed = seed;
			XOffset = xOffset;
			YOffset = yOffset;
			Octaves = octaves;
			Lacunarity = lacunarity;
			Persistence = persistence;
			InitialAmplitude = initialAmplitude;
			InitialFrequency = initialFrequency;
			Redistribution = redistribution;
			TerraceCount = terraceCount;
			NormaliseOutput = normaliseOutput;
		}

		public int Width { get; set; }
		public int Height { get; set; }
		public long Seed { get; set; }
		public double XOffset { get; set; }
		public double YOffset { get; set; }
		public int Octaves { get; set; }
		public double Lacunarity { get; set; }
		public double Persistence { get; set; }
		public double InitialAmplitude { get; set; }
		public double InitialFrequency { get; set; }
		public double Redistribution { get; set; }
		public int TerraceCount { get; set; }
		public bool NormaliseOutput { get; set; }
	}
}