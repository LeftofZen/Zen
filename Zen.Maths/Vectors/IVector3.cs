using System.Numerics;

namespace Zen.Maths.Vectors
{
    public interface IVector3<T> where T : INumber<T>, INumberBase<T>
    {
        T X { get; set; }
        T Y { get; set; }
        T Z { get; set; }
    }
}