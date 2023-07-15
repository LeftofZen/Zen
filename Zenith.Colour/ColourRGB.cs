using Zenith.Maths.Vectors;

namespace Zenith.Colour
{
	public record struct ColourRGB(float R, float G, float B) : IVector3<float>
	{
		// IVector3
		public float X { get => R; set => R = value; }
		public float Y { get => G; set => G = value; }
		public float Z { get => B; set => B = value; }

		public ColourHSB AsHSB()
			=> ColourSpaceConverter.RGBtoHSB(this);

		public static readonly ColourRGB None = new() { R = -1f, G = -1f, B = -1f };
	}
}
