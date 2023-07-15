namespace Zenith.Colour
{
	public static class ColourHelpers
	{
		public static ColourRGB AverageRGB(IEnumerable<ColourRGB> ColourRGBs)
		{
			var r = 0f;
			var g = 0f;
			var b = 0f;

			foreach (var c in ColourRGBs)
			{
				r += c.R;
				g += c.G;
				b += c.B;
			}

			var count = ColourRGBs.Count();
			r /= count;
			g /= count;
			b /= count;

			return new() { R = r, G = g, B = b };
		}
	}
}
