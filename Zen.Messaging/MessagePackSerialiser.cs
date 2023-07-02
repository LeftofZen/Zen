using MessagePack;

namespace Zen.Messaging
{
	public class MessagePackSerialiser : IMessageStreamSerialiser<IMessage>
	{
		public byte[] Serialise(IMessage msg)
			=> MessagePackSerializer.Serialize(msg.GetType(), msg);
		public byte[] Serialise<T>(T msg) => throw new NotImplementedException();
	}
}
