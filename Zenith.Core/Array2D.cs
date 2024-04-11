namespace Zenith.Core
{
	public class Array2D<T>
	{
		public T[,] Data { get; protected set; }

		public Array2D(T[,] data)
			=> Data = data;

		public Array2D(int width, int height)
			=> Data = new T[width, height];

		public int Width
			=> Data.GetLength(0);

		public int Height
			=> Data.GetLength(1);

		public T this[int x, int y]
		{
			get => Data[x, y];
			set => Data[x, y] = value;
		}

		public void Fill(T val)
		{
			Enumerate((img, x, y) => img[x, y] = val);
		}

		public void Enumerate(Action<Array2D<T>, int, int> func)
		{
			Enumerate(func, 0, 0, Width, Height);
		}

		public IEnumerable<(int X, int Y)> GetCellsInRect(int x, int y, int width, int height)
		{
			for (var ey = Math.Clamp(y, 0, Height); y < Math.Clamp(y + height, 0, Height); ++y)
			{
				for (var ex = Math.Clamp(x, 0, Height); x < Math.Clamp(x + height, 0, Height); ++x)
				{
					yield return (ex, ey);
				}
			}
		}

		public bool IsValid(int x, int y)
			=> !(x < 0 || y < 0 || x >= Width || y >= Height);

		public void Enumerate(Action<Array2D<T>, int, int> func, int x, int y, int width, int height)
		{
			for (var ey = y; y < y + height; ++y)
			{
				for (var ex = x; x < x + width; ++x)
				{
					func(this, ex, ey);
				}
			}
		}

		public IEnumerable<Point2> GetNeighbourPoints(Point2 p)
		{
			for (var x = -1; x < 2; ++x)
			{
				for (var y = -1; y < 2; ++y)
				{
					if (x != 0 || y != 0)
					{
						if (p.X + x >= 0 && p.X + x < Width && p.Y + y >= 0 && p.Y + y < Height)
						{
							yield return new Point2(p.X + x, p.Y + y);
						}
					}
				}
			}
		}
	}
}
