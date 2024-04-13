global using MessageToTypeLookup = System.Collections.Generic.IDictionary<uint, System.Type>;
global using MessageToNetworkLookup = System.Collections.Generic.IDictionary<System.Type, uint>;
using System.Runtime.InteropServices;
using MessagePack;

namespace Zenith.Messaging
{
	// all messages in this library work like so
	// |-----------------|----------------------------------------|
	// |     Header      |                 IMessage               |
	// || Type | Length ||| Type || RequiresLogin || ... body ... |
	//
	// we always parse header first which is always 8 bytes
	// type is 4 bytes, representing the body message type, which is user defined
	// length is the length of the body only (not including header) in bytes

	public interface IMessageLookup
	{
		public MessageToTypeLookup ToType { get; }

		public MessageToNetworkLookup ToNetwork { get; }
	}

	public interface IMessageLookup<T> where T : struct, Enum
	{
		public IDictionary<T, Type> ToType { get; }

		public IDictionary<Type, T> ToNetwork { get; }
	}

	public class MessageLookupBase(MessageToTypeLookup toType, MessageToNetworkLookup toNetwork) : IMessageLookup
	{
		public MessageToTypeLookup ToType { get; init; } = toType.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		public MessageToNetworkLookup ToNetwork { get; init; } = toNetwork;
	}

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

	public interface IMessage
	{
		uint Type { get; }
		bool RequiresLogin { get; }
	}

	public interface IClientId
	{
		public Guid ClientId { get; init; }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = Constants.MessageHeaderSize)] // MessageHeaderSize must == sizeof(Header)
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public record struct Header(uint Type, uint Length);
}
