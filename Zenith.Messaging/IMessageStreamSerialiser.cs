namespace Zenith.Messaging
{
	public interface IMessageStreamSerialiser<T> where T : IMessage
	{
		byte[] Serialise(T msg);
	}
}
