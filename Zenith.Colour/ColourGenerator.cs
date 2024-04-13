using System.Diagnostics;
using System.Numerics;
using Zenith.Core;

namespace Zenith.Colour
{
	public static class ColourGenerator
	{
		static readonly Random rnd = new(1);

		public static HashSet<ColourRGB> GenerateColours_RGB_All()
			=> GenerateColours_Uniform<ColourRGB>((int)Math.Pow(2, 24));

		public static HashSet<ColourRGB> GenerateColours_RGB_Uniform(int pixelCount)
			=> GenerateColours_Uniform<ColourRGB>(pixelCount);

		public static HashSet<ColourHSB> GenerateColours_HSB_Uniform(int pixelCount)
			=> GenerateColours_Uniform<ColourHSB>(pixelCount);

		public static HashSet<ColourRGB> GenerateColours_HSB_Uniform_RGB(int pixelCount)
		{
			var colours = GenerateColours_Uniform<ColourHSB>(pixelCount, DefaultDomain).Select(c => c.AsRGB()).ToHashSet();
			while (colours.Count < pixelCount)
			{
				var col = GenColour<ColourHSB>(DefaultDomain).AsRGB();
				_ = colours.Add(col);
			}

			Verify.AreEqual(colours.Count, pixelCount);
			return colours;
		}

		public static HashSet<T> GenerateColours_Uniform<T>(int pixelCount) where T : IVector3<float>, new()
			=> GenerateColours_Uniform<T>(pixelCount, DefaultDomain);

		public static HashSet<ColourRGB> GenerateColours_RGB_Pastel(int pixelCount)
			=> GenerateColours_Uniform<ColourRGB>(pixelCount, PastelDomainRGB);

		public static HashSet<ColourHSB> GenerateColours_HSB_Pastel(int pixelCount)
			=> GenerateColours_Uniform<ColourHSB>(pixelCount, PastelDomainHSB);

		public static HashSet<ColourRGB> GenerateColours_HSB_Pastel_RGB(int pixelCount)
		{
			var colours = GenerateColours_Uniform<ColourHSB>(pixelCount, PastelDomainHSB).Select(c => c.AsRGB()).ToHashSet();
			while (colours.Count < pixelCount)
			{
				var col = GenColour<ColourHSB>(PastelDomainHSB).AsRGB();
				_ = colours.Add(col);
			}

			Verify.AreEqual(colours.Count, pixelCount);
			return colours;
		}

		public static HashSet<T> GenerateColours_Uniform<T>(int pixelCount, DomainBound3<float> domainBounds) where T : IVector3<float>, new()
		{
			// validate colour space vs pixelCount
			var maxColours = domainBounds.Size * Math.Pow(256, 3);
			Verify.LessThanOrEqualTo(pixelCount, maxColours, $"Requested {pixelCount} colours but only found {maxColours} valid colours in RGB space for the given domain {domainBounds}");

			Console.WriteLine("Generating Colours");

			if (pixelCount == 0)
			{
				Console.WriteLine("no pixels");
				return [];
			}

			var colours = new HashSet<T>(pixelCount);

			var stepsPerChannel = (int)Math.Pow(pixelCount, 1f / 3f); // assumes a 3D colour space
			var stepSizeX = domainBounds.X.Size / (stepsPerChannel - 1); // subtract 1 because range is inclusive - 3 steps is [0, 0.5, 1]
			var stepSizeY = domainBounds.Y.Size / (stepsPerChannel - 1); // subtract 1 because range is inclusive - 3 steps is [0, 0.5, 1]
			var stepSizeZ = domainBounds.Z.Size / (stepsPerChannel - 1); // subtract 1 because range is inclusive - 3 steps is [0, 0.5, 1]

			var xSteps = stepsPerChannel;
			var ySteps = stepsPerChannel;
			var zSteps = stepsPerChannel;

			for (var x = 0; x < xSteps; ++x)
			{
				for (var y = 0; y < ySteps; ++y)
				{
					for (var z = 0; z < zSteps; ++z)
					{
						var hh = (stepSizeX * x) + domainBounds.X.Lower;
						var ss = (stepSizeY * y) + domainBounds.Y.Lower;
						var bb = (stepSizeZ * z) + domainBounds.Z.Lower;

						var c = new T() { X = hh, Y = ss, Z = bb };

						if (!colours.Add(c))
						{
							Trace.Assert(false, "duplicate colour detected", c.ToString());
						}
					}
				}
			}

			GenAdditionalColours(colours, pixelCount, domainBounds);

			Verify.AreEqual(colours.Count, pixelCount);

			return colours;
		}

		private static void GenAdditionalColours<T>(HashSet<T> setOfAllColours, int pixelCount, DomainBound3<float> domainBounds) where T : IVector3<float>, new()
		{
			while (setOfAllColours.Count < pixelCount)
			{
				var col = GenColour<T>(domainBounds);
				_ = setOfAllColours.Add(col);
			}
		}

		private static T GenColour<T>(DomainBound3<float> domainBounds) where T : IVector3<float>, new()
		{
			return new T()
			{
				X = (float)((rnd.NextDouble() * domainBounds.X.Size) + domainBounds.X.Lower),
				Y = (float)((rnd.NextDouble() * domainBounds.Y.Size) + domainBounds.Y.Lower),
				Z = (float)((rnd.NextDouble() * domainBounds.Z.Size) + domainBounds.Z.Lower),
			};
		}

		public static bool IsPastel(ColourRGB colourRGB)
			=> IsPastel(colourRGB.AsHSB());

		public static bool IsPastel(ColourHSB colourHSB)
			=> colourHSB.Hue >= PastelDomainHSB.X.Lower
			&& colourHSB.Hue <= PastelDomainHSB.X.Upper
			&& colourHSB.Saturation >= PastelDomainHSB.Y.Lower
			&& colourHSB.Saturation <= PastelDomainHSB.Y.Upper
			&& colourHSB.Brightness >= PastelDomainHSB.Z.Lower
			&& colourHSB.Brightness <= PastelDomainHSB.Z.Upper;

		public static DomainBound3<float> PastelDomainHSB
			=> new(new(0f, 1f), new(0.2f, 0.5f), new(0.5f, 0.8f));
		public static DomainBound3<float> PastelDomainRGB
			=> new(new(0.5f, 0.9f), new(0.5f, 0.9f), new(0.5f, 0.9f));

		public static DomainBound3<float> DefaultDomain
			=> new(new(0f, 1f), new(0f, 1f), new(0f, 1f));
	}

	public record DomainBound<T>(T Lower, T Upper) where T : INumber<T>
	{
		public T Size => Upper - Lower;
	}

	// could probably use IVector3 instead
	public record DomainBound3<T>(DomainBound<T> X, DomainBound<T> Y, DomainBound<T> Z) where T : INumber<T>
	{
		public T Size
			=> X.Size * Y.Size * Z.Size;
	}
}
