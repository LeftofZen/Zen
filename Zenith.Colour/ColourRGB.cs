using Zenith.Maths.Vectors;

namespace Zenith.Colour
{
	public record struct ColourRGB(int R, int G, int B) : IVector3<int>
	{
		// IVector3
		public int X { get => R; set => R = value; }
		public int Y { get => G; set => G = value; }
		public int Z { get => B; set => B = value; }

		public ColourHSB AsHSB()
			=> ColourSpaceConverter.RGBtoHSB(this);

		public static readonly ColourRGB None = new() { R = -1, G = -1, B = -1 };
	}
}
