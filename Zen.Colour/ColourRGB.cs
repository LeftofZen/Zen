using Zen.Maths.Vectors;

namespace Zen.Colour
{
	public record ColourRGB : IVector3<int>
	{
		public int R { get; set; }
		public int G { get; set; }
		public int B { get; set; }

		// IVector3
		public int X { get => R; set => R = value; }
		public int Y { get => G; set => G = value; }
		public int Z { get => B; set => B = value; }

		public ColourHSB AsHSB()
			=> ColourSpaceConverter.RGBtoHSB(this);

		public static readonly ColourRGB None = new() { R = -1, G = -1, B = -1 };
	}
}
