using Zen.Maths.Vectors;

namespace Zen.Colour
{
	public record ColourHSB : IVector3<float>
	{
		public float Hue { get; set; }
		public float Saturation { get; set; }
		public float Brightness { get; set; }

		// IVector3
		public float X { get => Hue; set => Hue = value; }
		public float Y { get => Saturation; set => Saturation = value; }
		public float Z { get => Brightness; set => Brightness = value; }

		public ColourRGB AsRGB()
			=> ColourSpaceConverter.HSBtoRGB(this);
	}
}
