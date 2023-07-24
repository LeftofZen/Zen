using System.Drawing;
using Zenith.Colour;

namespace Zenith.System.Drawing
{
	public static class ColourExtensions
	{
		public static ColourRGB ToColourRGB(this Color c)
			=> new() { R = c.R / 255f, G = c.G / 255f, B = c.B / 255f };

		public static Color ToSystemColor(this ColourRGB ColourRGB)
			=> Color.FromArgb((byte)(ColourRGB.R * 255), (byte)(ColourRGB.G * 255), (byte)(ColourRGB.B * 255));
	}
}
