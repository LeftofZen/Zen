namespace Zenith.ProceduralGeneration
{
	// didn't use records here because records don't bind to propertygrid in winforms :|
	public class DiamondSquareParams
	{
		public DiamondSquareParams(int width, int height, long seed, double roughness, double? initialValue)
		{
			Width = width;
			Height = height;
			Seed = seed;
			Roughness = roughness;
			InitialValue = initialValue;
		}

		public int Width { get; set; }
		public int Height { get; set; }
		public long Seed { get; set; } = 0;
		public double Roughness { get; set; } = 1.0;
		public double? InitialValue { get; set; } = null;
	}
}