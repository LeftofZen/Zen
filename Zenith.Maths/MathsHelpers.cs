using System.Numerics;
using Zenith.Core;
using Zenith.Maths.Points;
using Zenith.Maths.Vectors;

namespace Zenith.Maths
{
	public static class MathsHelpers
	{
		public static class Distance
		{
			public static T EuclideanSquared<T>(IVector3<T> a, IVector3<T> b) where T : INumber<T>, INumberBase<T>
				=> ((a.X - b.X) * (a.X - b.X))
				 + ((a.Y - b.Y) * (a.Y - b.Y))
				 + ((a.Z - b.Z) * (a.Z - b.Z));

			public static T Euclidean<T>(IVector3<T> a, IVector3<T> b) where T : INumber<T>, INumberBase<T>, IRootFunctions<T>
				=> T.Sqrt(EuclideanSquared(a, b));

			public static T Manhattan<T>(IVector3<T> a, IVector3<T> b) where T : INumber<T>, INumberBase<T>
				=> T.Abs(a.X - b.X)
				 + T.Abs(a.Y - b.Y)
				 + T.Abs(a.Z - b.Z);

			#region Point2

			public static float EuclideanSquared(Point2 a, Point2 b)
				=> ((a.X - b.X) * (a.X - b.X))
				 + ((a.Y - b.Y) * (a.Y - b.Y));

			public static float Euclidean(Point2 a, Point2 b)
				=> (float)Math.Sqrt(EuclideanSquared(a, b));

			public static float Euclidean(Point2 a, Point2 b, float maxDistance)
				=> (float)Math.Sqrt(EuclideanSquared(a, b) / maxDistance);

			public static int Manhattan(Point2 a, Point2 b)
				=> Math.Abs(a.X - b.X)
				 + Math.Abs(a.Y - b.Y);

			#endregion
		}

		public static T Rescale<T>(T val, T min, T max) where T : INumber<T>
			=> (val * (min - max)) + min;

		public static T Lerp<T, TLerp>(T start, T end, TLerp amount) where T : INumber<T>, IMultiplyOperators<T, TLerp, T> where TLerp : IFloatingPoint<TLerp>
			=> start + ((end - start) * amount);

		public static TVector Lerp<T, TLerp, TVector>(TVector start, TVector end, TLerp amount)
			where T : INumber<T>, IMultiplyOperators<T, TLerp, T>
			where TLerp : IFloatingPoint<TLerp>
			where TVector : IVector<T>
		{
			var outVec = default(TVector);
			for (var i = 0; i < start.Components.Length; ++i)
			{
				outVec!.Components[i] = Lerp(start.Components[i], end.Components[i], amount);
			}
			return outVec!;
		}
	}
}