namespace Zenith.Messaging
{
	public interface IMessageStreamDeserialiser<T> where T : IMessage
	{
		T? Deserialise(Header hdr, byte[] bytes);
	}
}
