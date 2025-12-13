global using MessageToTypeLookup = System.Collections.Generic.IDictionary<uint, System.Type>;
global using MessageToNetworkLookup = System.Collections.Generic.IDictionary<System.Type, uint>;

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
}
