using System.Diagnostics;
using System.Numerics;
using Zenith.Core;
using Zenith.Maths.Vectors;

namespace Zenith.Colour
{
	public static class ColourGenerator
	{
		public static HashSet<ColourRGB> GenerateColours_RGB_All()
			=> GenerateColours_Uniform<ColourRGB>((int)Math.Pow(2, 24));

		public static HashSet<ColourRGB> GenerateColours_RGB_Uniform(int pixelCount)
			=> GenerateColours_Uniform<ColourRGB>(pixelCount);

		public static HashSet<ColourHSB> GenerateColours_HSB_Uniform(int pixelCount)
			=> GenerateColours_Uniform<ColourHSB>(pixelCount);

		public static HashSet<T> GenerateColours_Uniform<T>(int pixelCount) where T : IVector3<float>, new()
			=> GenerateColours_Uniform<T>(pixelCount, DefaultDomain);

		public static HashSet<T> GenerateColours_Uniform<T>(int pixelCount, DomainBound3<float> domainBounds) where T : IVector3<float>, new()
		{
			if (typeof(T) == typeof(ColourRGB))
			{
				// validate colour space vs pixelCount
				var totalValidColours = (int)(domainBounds.X.Size * 255) * (int)(domainBounds.Y.Size * 255) * (int)(domainBounds.Z.Size * 255); // do not factor the 255 out - we must multiply in each dimension individually - combining dimensions mayb produce off-by-one errors
				Verify.LessThanOrEqualTo(pixelCount, totalValidColours, $"Requested {pixelCount} colours but only found {totalValidColours} valid colours in RGB space for the given domain {domainBounds}");
			}

			Console.WriteLine("Generating Colours");

			if (pixelCount == 0)
			{
				Console.WriteLine("no pixels");
				return new HashSet<T>();
			}

			var setOfAllColours = new HashSet<T>(pixelCount);
			var rnd = new Random(1);

			var stepsPerChannel = (int)Math.Pow(pixelCount, 1f / 3f); // assumes a 3D colour space
			var stepSizeX = (domainBounds.X.Size / (stepsPerChannel - 1)) + domainBounds.X.Lower; // subtract 1 because range is inclusive - 3 steps is [0, 0.5, 1]
			var stepSizeY = (domainBounds.Y.Size / (stepsPerChannel - 1)) + domainBounds.Y.Lower; // subtract 1 because range is inclusive - 3 steps is [0, 0.5, 1]
			var stepSizeZ = (domainBounds.Z.Size / (stepsPerChannel - 1)) + domainBounds.Z.Lower; // subtract 1 because range is inclusive - 3 steps is [0, 0.5, 1]

			var xSteps = stepsPerChannel;
			var ySteps = stepsPerChannel;
			var zSteps = stepsPerChannel;

			for (var x = 0; x < xSteps; ++x)
			{
				for (var y = 0; y < ySteps; ++y)
				{
					for (var z = 0; z < zSteps; ++z)
					{
						var hh = stepSizeX * x;
						var ss = stepSizeY * y;
						var bb = stepSizeZ * z;

						var c = new T() { X = hh, Y = ss, Z = bb };

						if (!setOfAllColours.Add(c))
						{
							Trace.Assert(false, "duplicate colour detected", c.ToString());
						}
					}
				}
			}

			while (setOfAllColours.Count < pixelCount)
			{
				var col = new T()
				{
					X = (float)rnd.NextDouble(),
					Y = (float)rnd.NextDouble(),
					Z = (float)rnd.NextDouble(),
				};

				_ = setOfAllColours.Add(col);
			}

			Trace.Assert(setOfAllColours.Count == pixelCount);

			return setOfAllColours;
		}

		public static HashSet<ColourRGB> GenerateColours_RGB_Pastel(int pixelCount)
			=> GenerateColours_Uniform<ColourRGB>(pixelCount, PastelDomainRGB);
		public static HashSet<ColourHSB> GenerateColours_HSB_Pastel(int pixelCount)
			=> GenerateColours_Uniform<ColourHSB>(pixelCount, PastelDomainHSB);

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
			=> new(new(0f, 1f), new(0.2f, 0.5f), new(0.5f, 1f));
		public static DomainBound3<float> PastelDomainRGB
			=> new(new(0.6f, 0.9f), new(0.6f, 0.9f), new(0.6f, 0.9f));

		public static DomainBound3<float> DefaultDomain
			=> new(new(0f, 1f), new(0f, 1f), new(0f, 1f));
	}

	public record DomainBound<T>(T Lower, T Upper) where T : INumber<T>
	{
		public T Size => Upper - Lower;
	}

	// could probably use IVector3 instead
	public record DomainBound3<T>(DomainBound<T> X, DomainBound<T> Y, DomainBound<T> Z) where T : INumber<T>;
}
