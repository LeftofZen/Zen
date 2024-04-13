using NUnit.Framework;
using Zenith.Colour;

namespace Zenith.Testing.Colour
{
	[TestFixture]
	public class ColourEqualiserTests
	{
		[Test]
		public void Equalise()
		{
			// arrange
			var colours = new HashSet<ColourRGB>();
			_ = colours.Add(new ColourRGB(1f, 0.2f, 0.4f));
			_ = colours.Add(new ColourRGB(0.01f, 0.48f, 0.86f));
			_ = colours.Add(new ColourRGB(0f, 1f, 0.13f));

			// act
			var result = ColourEqualiser.Equalise(colours);

			// assert
			Assert.That(0f, Is.EqualTo(result.Min(c => c.R)));
			Assert.That(0f, Is.EqualTo(result.Min(c => c.G)));
			Assert.That(0f, Is.EqualTo(result.Min(c => c.B)));

			Assert.That(1f, Is.EqualTo(result.Max(c => c.R)));
			Assert.That(1f, Is.EqualTo(result.Max(c => c.G)));
			Assert.That(1f, Is.EqualTo(result.Max(c => c.B)));
		}
	}
}
