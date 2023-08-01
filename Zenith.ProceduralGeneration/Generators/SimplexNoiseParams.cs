namespace Zenith.ProceduralGeneration
{
	// didn't use records here because records don't bind to propertygrid in winforms :|
	public class SimplexNoiseParams
	{
		public SimplexNoiseParams(int width, int height, long seed, double xOffset, double yOffset, int octaves, double lacunarity, double persistence, double initialAmplitude, double initialFrequency, double redistribution, bool useTerracing, int terraceCount, bool normaliseOutput)
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
		public long Seed { get; set; } = 0;
		public double XOffset { get; set; } = 0;
		public double YOffset { get; set; } = 0;
		public int Octaves { get; set; } = 8;
		public double Lacunarity { get; set; } = 3.0;
		public double Persistence { get; set; } = 0.5;
		public double InitialAmplitude { get; set; } = 1;
		public double InitialFrequency { get; set; } = 0.005;
		public double Redistribution { get; set; } = 1;
		public int TerraceCount { get; set; } = 10;
		public bool NormaliseOutput { get; set; } = true;
	}
}