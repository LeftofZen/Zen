using Zen.Maths.Vectors;

namespace Zen.Colour
{
	public record struct ColourHSB(float Hue, float Saturation, float Brightness) : IVector3<float>
	{
		// IVector3
		public float X { get => Hue; set => Hue = value; }
		public float Y { get => Saturation; set => Saturation = value; }
		public float Z { get => Brightness; set => Brightness = value; }

		public ColourRGB AsRGB()
			=> ColourSpaceConverter.HSBtoRGB(this);
	}
}
