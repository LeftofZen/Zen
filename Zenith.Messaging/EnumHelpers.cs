namespace Zenith.Messaging
{
	public static class EnumHelpers
	{
		public static unsafe uint GetUnderlyingValueAsUint<T>(T value) where T : struct, Enum
		{
			var tr = __makeref(value);
			var ptr = **(IntPtr**)&tr;
			return (uint)*(int*)ptr;
		}

		public static unsafe T GetEnumValueFromUint<T>(uint value) where T : struct, Enum
			=> *(T*)&value; // Directly map the uint to a pointer of the enum type
	}
}
