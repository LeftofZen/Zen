using System.Numerics;

namespace Zenith.Core
{
	public interface IVector2<T> : IVector<T> where T : INumber<T>, INumberBase<T>
	{
		T X { get; set; }
		T Y { get; set; }

		T[] IVector<T>.Components => new T[] { X, Y };
	}
}