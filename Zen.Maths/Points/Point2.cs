using Zen.Maths.Vectors;

namespace Zen.Maths.Points
{
	public record Point2 : IVector2<int>
	{
		public int X { get; set; }
		public int Y { get; set; }

		public static readonly Point2 Zero = new(0, 0);
		public static readonly Point2 One = new(1, 1);

		public Point2(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}