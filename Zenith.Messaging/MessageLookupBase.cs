namespace Zenith.Messaging
{
	public class MessageLookupBase(MessageToTypeLookup toType, MessageToNetworkLookup toNetwork) : IMessageLookup
	{
		public MessageToTypeLookup ToType { get; init; } = toType.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		public MessageToNetworkLookup ToNetwork { get; init; } = toNetwork;
	}

	public class MessageLookupBase<T> : IMessageLookup<T> where T : struct, Enum
	{
		//public MessageLookupBase(IDictionary<uint, Type> toType, IDictionary<Type, T> toNetwork)
		//{
		//	var underlyingType = Enum.GetUnderlyingType(typeof(T));
		//	if (underlyingType != typeof(int) && underlyingType != typeof(uint))
		//	{
		//		throw new ArgumentException("Message type enum didn't have underlying type of int or uint", nameof(toType));
		//	}

		//	ToType = toType.ToDictionary(kvp => EnumHelpers.GetUnderlyingValueAsUint<T>(kvp.Key), kvp => kvp.Value);
		//	ToNetwork = toNetwork;
		//}

		public MessageLookupBase(IDictionary<T, Type> toType, IDictionary<Type, T> toNetwork)
		{
			var underlyingType = Enum.GetUnderlyingType(typeof(T));
			if (underlyingType != typeof(int) && underlyingType != typeof(uint))
			{
				throw new ArgumentException("Message type enum didn't have underlying type of int or uint", nameof(toType));
			}

			ToType = toType.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			ToNetwork = toNetwork;
		}

		public IDictionary<T, Type> ToType { get; init; }

		public IDictionary<Type, T> ToNetwork { get; init; }
	}
}
