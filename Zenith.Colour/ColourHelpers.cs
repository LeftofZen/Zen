namespace Zenith.Colour
{
	public static class ColourHelpers
	{
		public static ColourRGB AverageRGB(IEnumerable<ColourRGB> ColourRGBs)
		{
			var r = 0;
			var g = 0;
			var b = 0;

			foreach (var c in ColourRGBs)
			{
				r += c.R;
				g += c.G;
				b += c.B;
			}

			r /= ColourRGBs.Count();
			g /= ColourRGBs.Count();
			b /= ColourRGBs.Count();

			return new() { R = r, G = g, B = b };
		}
	}
}
