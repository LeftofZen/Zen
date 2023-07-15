using System.Numerics;

namespace Zenith.Maths.Vectors
{
	public interface IVector2<T> where T : INumber<T>, INumberBase<T>
	{
		T X { get; set; }
		T Y { get; set; }
	}
}