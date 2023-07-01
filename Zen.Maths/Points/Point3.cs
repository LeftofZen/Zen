using Zen.Maths.Vectors;

namespace Zen.Maths.Points
{
	public record Point3 : IVector3<int>
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public static readonly Point3 Zero = new(0, 0, 0);
		public static readonly Point3 One = new(1, 1, 1);

		public Point3(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}