using MessagePack;

namespace Zenith.Messaging
{
	public class MessagePackDeserialiser<TEnum>(IMessageLookup<TEnum> lookup)
		: IMessageStreamDeserialiser<IMessage> where TEnum : struct, Enum
	{
		public IMessageLookup<TEnum> Lookup { get; } = lookup;

		public IMessage? Deserialise(Header hdr, byte[] bytes)
			=> (IMessage?)MessagePackSerializer.Deserialize(Lookup.ToType[EnumHelpers.GetEnumValueFromUint<TEnum>(hdr.Type)], bytes);
	}
}
