namespace Zenith.ProceduralGeneration
{
	public static class NoiseHelpers
	{
		public static void Terrace(this double[,] data, int count)
		{
			var min = double.MaxValue;
			var max = double.MinValue;
			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					min = Math.Min(min, data[x, y]);
					max = Math.Max(max, data[x, y]);
				}
			}

			var range = max - min;
			var terraceHeight = range / (count - 1); // -1 because there's an implicit extra terrace at the end of the range

			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					var terraceNumber = (int)((data[x, y] - min) / terraceHeight);
					data[x, y] = min + (terraceNumber * terraceHeight);
				}
			}
		}

		public static void Normalise(this double[,] data)
		{
			var min = double.MaxValue;
			var max = double.MinValue;
			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					min = Math.Min(min, data[x, y]);
					max = Math.Max(max, data[x, y]);
				}
			}

			var range = max - min;
			var xx = 1f / range;

			for (var y = 0; y < data.GetLength(1); y++)
			{
				for (var x = 0; x < data.GetLength(0); x++)
				{
					data[x, y] = (data[x, y] - min) * xx;
				}
			}
		}
	}
}