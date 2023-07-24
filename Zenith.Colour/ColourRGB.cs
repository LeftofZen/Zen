using Zenith.Core;

namespace Zenith.Colour
{
	public record struct ColourRGB : IColour
	{
		public ColourRGB(float r, float g, float b)
		{
			R = r;
			G = g;
			B = b;
		}

		public float R { get => r; set { Verify.InRangeInclusive(value, 0f, 1f); r = value; } }
		public float G { get => g; set { Verify.InRangeInclusive(value, 0f, 1f); g = value; } }
		public float B { get => b; set { Verify.InRangeInclusive(value, 0f, 1f); b = value; } }

		float r;
		float g;
		float b;

		// IVector3
		public float X { get => R; set => R = value; }
		public float Y { get => G; set => G = value; }
		public float Z { get => B; set => B = value; }

		public ColourHSB AsHSB()
			=> ColourSpaceConverter.RGBtoHSB(this);

		#region Predefined Colours

		public static readonly ColourRGB Black = new(0, 0, 0);
		public static readonly ColourRGB White = new(1, 1, 1);

		public static readonly ColourRGB Red = new(1, 0, 0);
		public static readonly ColourRGB Green = new(0, 1, 0);
		public static readonly ColourRGB Blue = new(0, 0, 1);

		public static readonly ColourRGB Yellow = new(1, 1, 0);
		public static readonly ColourRGB Cyan = new(0, 1, 1);
		public static readonly ColourRGB Magenta = new(1, 0, 1);

		#endregion
	}
}
