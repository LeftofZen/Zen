namespace Zenith.Core
{
	public record Vector2 : IVector2<float>
	{
		public float X { get; set; }
		public float Y { get; set; }

		public static readonly Vector2 Zero = new(0f, 0f);
		public static readonly Vector2 One = new(1f, 1f);

		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}
	}
}