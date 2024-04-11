using System.Numerics;

namespace Zenith.Core
{
	public interface IVector4<T> : IVector<T> where T : INumber<T>, INumberBase<T>
	{
		T X { get; set; }
		T Y { get; set; }
		T Z { get; set; }
		T W { get; set; }

		T[] IVector<T>.Components => new T[] { X, Y, Z, W };
	}
}