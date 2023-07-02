using System.Runtime.InteropServices;
using MessagePack;

namespace Zen.Messaging
{
	// all messages in this library work like so
	// |-----------------|----------------------------------------|
	// |     Header      |                 IMessage               |
	// || Type | Length ||| Type || RequiresLogin || ... body ... |
	//
	// we always parse header first which is always 8 bytes
	// type is 4 bytes, representing the body message type, which is user defined
	// length is the length of the body only (not including header) in bytes

	public interface IMessageLookup<T> where T : struct, Enum
	{
		public IDictionary<uint, Type> ToType { get; }

		public IDictionary<Type, T> ToNetwork { get; }
	}

	public class MessageLookupBase<T> : IMessageLookup<T> where T : struct, Enum
	{
		public unsafe uint GetUnderlyingValue(T value)
		{
			var tr = __makeref(value);
			var ptr = **(IntPtr**)&tr;
			return (uint)*(int*)ptr;
		}

		public MessageLookupBase(IDictionary<T, Type> toType, IDictionary<Type, T> toNetwork)
		{
			var underlyingType = Enum.GetUnderlyingType(typeof(T));
			if (underlyingType != typeof(int) && underlyingType != typeof(uint))
			{
				throw new ArgumentException("Message type enum didn't have underlying type of int or uint", nameof(toType));
			}

			ToType = toType.ToDictionary(kvp => GetUnderlyingValue(kvp.Key), kvp => kvp.Value);
			ToNetwork = toNetwork;
		}

		public IDictionary<uint, Type> ToType { get; init; }

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
	public struct Header
	{
		public uint Type { get; init; }
		public uint Length { get; set; }
	}
}
