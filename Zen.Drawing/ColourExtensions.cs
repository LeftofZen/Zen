using System.Drawing;
using Zen.Colour;

namespace Zen.System.Drawing
{
	public static class ColourRGBExtensions
	{
		public static ColourRGB ToColourRGB(this Color c)
			=> new() { R = (int)(c.R / 255f), G = (int)(c.G / 255f), B = (int)(c.B / 255f) };

		public static Color ToSystemColor(this ColourRGB ColourRGB)
			=> Color.FromArgb(ColourRGB.R * 255, ColourRGB.G * 255, ColourRGB.B * 255);
	}
}
