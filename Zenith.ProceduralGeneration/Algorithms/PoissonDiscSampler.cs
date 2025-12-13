using Zenith.Core;

namespace Zenith.ProceduralGeneration.Algorithms
{
	public record struct Circle(float Radius, Vector2 Point);

	public static class PoissonDiscSampler
	{
		public static IEnumerable<Circle> Sample()
		{
			var points = new List<Circle>();
			return points;
		}
	}
}
