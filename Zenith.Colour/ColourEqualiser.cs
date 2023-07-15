namespace Zenith.Colour
{
	public static class ColourEqualiser
	{
		public static IEnumerable<ColourRGB> Equalise(IEnumerable<ColourRGB> colours)
		{
			var minR = float.MaxValue;
			var maxR = float.MinValue;

			var minG = float.MaxValue;
			var maxG = float.MinValue;

			var minB = float.MaxValue;
			var maxB = float.MinValue;

			foreach (var c in colours)
			{
				minR = Math.Min(minR, c.R);
				minG = Math.Min(minG, c.G);
				minB = Math.Min(minB, c.B);

				maxR = Math.Max(maxR, c.R);
				maxG = Math.Max(maxG, c.G);
				maxB = Math.Max(maxB, c.B);
			}

			var diffR = 1f / (maxR - minR);
			var diffG = 1f / (maxG - minG);
			var diffB = 1f / (maxB - minB);

			foreach (var c in colours)
			{
				var r = (c.R - minR) * diffR * 255;
				var g = (c.G - minG) * diffG * 255;
				var b = (c.B - minB) * diffB * 255;
				yield return new ColourRGB() { R = (int)r, G = (int)g, B = (int)b };
			}
		}
	}
}
