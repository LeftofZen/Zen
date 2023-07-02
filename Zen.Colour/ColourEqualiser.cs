namespace Zen.Colour
{
	public static class ColourEqualiser
	{
		public static IEnumerable<ColourRGB> Equalise(IEnumerable<ColourRGB> colours)
		{
			var minR = int.MaxValue;
			var maxR = int.MinValue;

			var minG = int.MaxValue;
			var maxG = int.MinValue;

			var minB = int.MaxValue;
			var maxB = int.MinValue;

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
				var r = (c.R - minR) * diffR;
				var g = (c.G - minG) * diffG;
				var b = (c.B - minB) * diffB;
				yield return new ColourRGB() { R = (int)r, G = (int)g, B = (int)b };
			}
		}
	}
}
