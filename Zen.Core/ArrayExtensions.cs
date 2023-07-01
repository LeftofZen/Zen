using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zen.Core
{
	public static class ArrayExtensions
	{
		/// <summary>
		/// Taken from https://github.com/dotnet/runtime/issues/47213#issuecomment-1544610811
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="value"></param>
		public static void Fill<T>(this Array array, T value)
		{
			// Skipping prologue with checks and whatnot. Also ignoring cases where the size of
			// the array exceeds int.MaxValue, feel free to check/chunk if you need to support that.

			ref var reference = ref MemoryMarshal.GetArrayDataReference(array);
			var span = MemoryMarshal.CreateSpan(ref Unsafe.As<byte, T>(ref reference), array.Length);

			span.Fill(value);
		}
	}
}