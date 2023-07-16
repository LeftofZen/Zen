using Zenith.Core;
using Zenith.Maths.Vectors;

namespace Zenith.Colour
{
	public record struct ColourHSB : IVector3<float>
	{
		public ColourHSB(float hue, float saturation, float brightness)
		{
			Hue = hue;
			Saturation = saturation;
			Brightness = brightness;
		}

		public float Hue { get => h; set { Verify.InRangeInclusive(value, 0f, 1f); h = value; } }
		public float Saturation { get => s; set { Verify.InRangeInclusive(value, 0f, 1f); s = value; } }
		public float Brightness { get => b; set { Verify.InRangeInclusive(value, 0f, 1f); b = value; } }

		float h;
		float s;
		float b;

		// IVector3
		public float X { get => Hue; set => Hue = value; }
		public float Y { get => Saturation; set => Saturation = value; }
		public float Z { get => Brightness; set => Brightness = value; }

		public ColourRGB AsRGB()
			=> ColourSpaceConverter.HSBtoRGB(this);
	}
}
