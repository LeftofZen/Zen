using System.Diagnostics;
using Zen.Linq;

namespace Zen.Colour
{
	public static class ColourGenerator
	{
		public static HashSet<ColourRGB> GenerateColours_RGB_All()
			=> GenerateColours_RGB_Uniform((int)Math.Pow(2, 24));

		public static HashSet<ColourRGB> GenerateColours_RGB_Uniform(int pixelCount)
		{
			Console.WriteLine("Generating ColourRGBs");

			if (pixelCount == 0)
			{
				Console.WriteLine("no pixels");
				return new HashSet<ColourRGB>();
			}

			var setOfAllColourRGBs = new HashSet<ColourRGB>(pixelCount);

			var stepsPerChannel = (int)Math.Pow(pixelCount, 1f / 3f);
			var stepSize = 1f / (stepsPerChannel - 1); // subtract 1 because range is inclusive - 3 steps is [0, 0.5, 1]
			var rSteps = stepsPerChannel;
			var gSteps = stepsPerChannel;
			var bSteps = stepsPerChannel;

			for (var r = 0; r < rSteps; ++r)
			{
				for (var g = 0; g < gSteps; ++g)
				{
					for (var b = 0; b < bSteps; ++b)
					{
						var rr = stepSize * r;
						var gg = stepSize * g;
						var bb = stepSize * b;

						var c = new ColourRGB() { R = (int)(rr * 255), G = (int)(gg * 255), B = (int)(bb * 255) };

						if (!setOfAllColourRGBs.Add(c))
						{
							Trace.Assert(false, "duplicate ColourRGB detected", c.ToString());
						}
					}
				}
			}

			Trace.Assert(setOfAllColourRGBs.Count == stepsPerChannel * stepsPerChannel * stepsPerChannel);

			var rnd = new Random(1);

			// leftover from non-integer cube root
			while (setOfAllColourRGBs.Count < pixelCount)
			{
				var rgb = new ColourRGB()
				{
					R = rnd.Next(0, 256),
					G = rnd.Next(0, 256),
					B = rnd.Next(0, 256)
				};

				_ = setOfAllColourRGBs.Add(rgb);
			}

			Trace.Assert(setOfAllColourRGBs.Count >= pixelCount);

			return setOfAllColourRGBs;
		}

		public static HashSet<ColourRGB> GenerateColourRGBs_HSB_Random(int pixelCount)
		{
			Console.WriteLine("Generating ColourRGBs");

			if (pixelCount == 0)
			{
				Console.WriteLine("no pixels");
				return new HashSet<ColourRGB>();
			}

			var setOfAllColourRGBs = new HashSet<ColourRGB>(pixelCount);
			var rnd = new Random(1);

			while (setOfAllColourRGBs.Count < pixelCount)
			{
				var hsb = new ColourHSB()
				{
					Hue = (float)(rnd.NextDouble() * 360),
					Saturation = (float)rnd.NextDouble(),
					Brightness = (float)rnd.NextDouble(),
				};

				_ = setOfAllColourRGBs.Add(hsb.AsRGB());
			}

			Trace.Assert(setOfAllColourRGBs.Count == pixelCount);

			return setOfAllColourRGBs;
		}

		public static HashSet<ColourRGB> GenerateColourRGBs_RGB_Pastel(int pixelCount)
		{
			var baseline = GenerateColours_RGB_All().ToList();
			baseline.Shuffle();

			var result = new List<ColourRGB>();
			var enumerator = baseline.GetEnumerator();
			do
			{
				var current = enumerator.Current;
				if (IsPastel(current))
				{
					result.Add(current);
				}
			}
			while (result.Count < pixelCount && enumerator.MoveNext());

			return result.ToHashSet();
		}

		public static bool IsPastel(ColourRGB colourRGB)
			=> IsPastel(colourRGB.AsHSB());

		public static bool IsPastel(ColourHSB colourHSB)
			=> colourHSB.Brightness > 0.5f && colourHSB.Saturation < 0.5f && colourHSB.Saturation > 0.2f;
	}
}
