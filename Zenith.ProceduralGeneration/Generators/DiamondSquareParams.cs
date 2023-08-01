namespace Zenith.ProceduralGeneration
{
	// didn't use records here because records don't bind to propertygrid in winforms :|
	public class DiamondSquareParams
	{
		public DiamondSquareParams(int width, int height, long seed = 0, double roughness = 1.0, double? initialValue = null)
		{
			Width = width;
			Height = height;
			Seed = seed;
			Roughness = roughness;
			InitialValue = initialValue;
		}

		public int Width { get; set; }
		public int Height { get; set; }
		public long Seed { get; set; }
		public double Roughness { get; set; }
		public double? InitialValue { get; set; }
	}
}