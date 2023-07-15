using System.Numerics;

namespace Zenith.Maths.Vectors
{
    public interface IVector4<T> where T : INumber<T>, INumberBase<T>
    {
        T W { get; set; }
        T X { get; set; }
        T Y { get; set; }
        T Z { get; set; }
    }
}