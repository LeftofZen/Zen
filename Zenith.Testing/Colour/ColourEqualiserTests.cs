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
			Assert.AreEqual(0f, result.Min(c => c.R));
			Assert.AreEqual(0f, result.Min(c => c.G));
			Assert.AreEqual(0f, result.Min(c => c.B));

			Assert.AreEqual(1f, result.Max(c => c.R));
			Assert.AreEqual(1f, result.Max(c => c.G));
			Assert.AreEqual(1f, result.Max(c => c.B));
		}
	}
}
